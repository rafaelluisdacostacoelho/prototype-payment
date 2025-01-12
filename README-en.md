# 1. Development Environment Setup

We need to configure NuGet and OpenSSH. The latter will be used to create a Certificate Authority (CA) that we will use to manage certificates, which is necessary for using gRPC in a consistent manner similar to a production environment.

## 1.1. OpenSSH

Configuring gRPC with TLS in a local environment (for development) typically involves generating self-signed certificates. The general idea is:

* **Create a local root Certificate Authority (CA)**.
* **Generate server certificates** and sign them using this root CA.
* *(Optional) Create client certificates** for mutual TLS (if you want to authenticate the client as well).
* **Configure** the gRPC server (e.g., Kestrel) to use the server certificate.
* **Configure** the gRPC client to trust the CA (or use a client certificate if it's mTLS).
* 
### 1.1.1. Create a Root Certificate Authority (CA)

The root CA will be used to sign the certificates we will use on the server (and client if necessary). It will have a key pair (private and public) and a certificate. It is important to **protect** the root CA's private key because it allows signing certificates on behalf of your "authority".

```sh
# Create the key pair and certificate for the root CA (self-signed):

openssl req -x509 -nodes -newkey rsa:2048 \
	-keyout RootCA.key \ 
    -out RootCA.crt \ 
    -days 365 \ 
    -subj "/C=BR/ST=SP/L=SaoPaulo/O=MinhaEmpresa/CN=DevRootCA"
```

- `-x509` indicates that we are generating a certificate (not just the CSR).
- `-nodes` prevents the private key from being encrypted with a password (this is common for development, but normally a password is used in production).
- `-subj` defines the basic fields for the certificate, such as country (`C`), state (`ST`), locality (`L`), organization (`O`), and **Common Name** (`CN`).

After this command, we will have two main files:

- `RootCA.key`: private key of the CA (store securely).
- `RootCA.crt`: the CA certificate that can be distributed.

### 1.1.2. Generate the Server Certificate

We will first create a "Certificate Signing Request" (CSR) and then use the CA to sign the request.

#### 1.1.2.1. Create Private Key and CSR

```sh
# 1) Generate the server's private key and CSR: 
openssl req -newkey rsa:2048 -nodes \ 
	-keyout Server.key \ 
	-out Server.csr \ 
	-subj "/C=BR/ST=SP/L=SaoPaulo/O=MinhaEmpresa/CN=localhost"
```

The `CN=localhost` is important for local development because we will access the server via `https://localhost`. In production, obviously, you would need to use the actual domain of your service.

#### 1.1.2.2. Sign the CSR with the Root CA

```sh
# 2) Sign the server's CSR with our root CA, generating the certificate 
openssl x509 -req -in Server.csr \ 
	-CA RootCA.crt \ 
	-CAkey RootCA.key \ 
	-CAcreateserial \ 
	-out server.crt \ 
	-days 365
```

This command generates the `Server.crt` file, which is the server certificate signed by our root CA.

> The `-CAcreateserial` parameter creates a `RootCA.srl` file (for serial number tracking of certificates). In a real environment, you might want to control this serial manually.

## 1.1.3. Create PFX File (Optional, but Common in .NET)

For use in .NET, it is **very common** to consolidate the private key and certificate into a `PFX` (PKCS#12) file. This allows Kestrel to load the certificate and key in a single file.

```sh
openssl pkcs12 -export \ 
	-out Server.pfx \ 
	-inkey Server.key \ 
	-in Server.crt \ 
	-certfile RootCA.crt \ 
	-passout pass:MinhaSenhaSegura
```

This will ask for the password (defined in `-passout`).

- `Server.pfx`: contains the server certificate + private key + CA certificate in the "chain".

In a **dev environment**, sometimes we use `-passout pass:` (blank password) to avoid entering it repeatedly. But if possible, use a password.

## 1.1.4. Configure gRPC Server (Kestrel) in .NET

### 1.1.4.1. In `Program.cs` (or `Startup.cs`)

In ASP.NET Core 5+, Kestrel can be configured like this:

```c#
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    // Load the .pfx certificate
                    var cert = new X509Certificate2("certs/server.pfx", "MinhaSenhaSegura");

                    options.ListenAnyIP(5001, listenOptions =>
                    {
                        listenOptions.UseHttps(cert);
                    });
                });

                webBuilder.UseStartup<Startup>();
            });
}
```

> Note that for **development**, you can also configure this via `appsettings.json`, but it is common to do it in code (or via secrets).

### 1.1.4.2. Configure the gRPC Client

The client configuration depends on the type of TLS authentication you want to use:

### 1.1.4.3. Only Validate the Server Certificate

In this case, just **trust** the root CA in the operating system. In a development environment, you can install the `rootCA.crt` locally so that Windows/macOS/Linux will recognize the certificate as trusted.

- Windows: just open `rootCA.crt`, click install, and place it in "Trusted Root Certification Authorities".
- macOS/Linux: use native tools (`security add-trusted-cert`, `update-ca-trust`, etc.).

With this, any client making a TLS call to a certificate signed by `DevRootCA` will trust it automatically.

In .NET (C#), if you are making gRPC calls, it can be something like:

```c#
using Grpc.Net.Client;
using Grpc.Core;
using System.Net.Http;

// Use the HTTPS channel to connect to the gRPC server
var httpHandler = new HttpClientHandler
{
    // If you haven't installed the rootCA in the system, you can allow (unsafe for prod)
    // ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, chain, errors) => true
};

using var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

// Now you can call your gRPC methods ...
```

> In **development**, we often "ignore" certificate errors by setting `ServerCertificateCustomValidationCallback = ... => true`, but it is not recommended for production. The ideal is to install the CA certificate and perform actual validation.

### 1.1.4.4. Mutual TLS (mTLS)

If you also need the **server to validate** the client, then you need to generate a client certificate (`client.crt` and `client.key`), sign it with the root CA, and then use this certificate on the client.

In .NET for gRPC, we can do something like:

```c#
var clientCertificate = new X509Certificate2("certs/client.pfx", "SenhaDoCliente");

var httpHandler = new HttpClientHandler();
httpHandler.ClientCertificates.Add(clientCertificate);

using var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});
```

And on the server side, you can configure client certificate requirements, but that involves more specific `ClientCertificateMode` settings in Kestrel.

## 1.1.5. Flow Summary

1. **Generate and store** the root CA (`.key` + `.crt`).
2. For each "role" (server, client, etc.), generate a CSR and sign it with the root CA.
3. Convert to `.pfx` if using in .NET (optional, but easier).
4. On the **server**, configure Kestrel to use the server's `.pfx`.
5. On the **client**, either trust the CA (installing it on the OS) or load the CA and perform manual certificate validation.

## 1.1.6. Extra Recommendations

- For **production**, use certificates issued by a trusted public CA (e.g., Let’s Encrypt, Digicert, etc.).
- If you plan to **automate** the certificate generation process for development, create scripts (`.sh` or `.ps1`) that run these OpenSSL commands, making it easier for new developers to onboard.
- Be cautious when using the same CA across different environments. For local dev, keep something easily revocable/discardable to avoid compromising security.

## Conclusion

In summary, setting up the digital certificate structure for use with gRPC in a local environment involves:

1. Creating a local CA.
2. Creating and signing server/client certificates with this CA.
3. Configuring the server (Kestrel) to use the server certificate.
4. Configuring the client to trust the CA or use a client certificate if needed (mTLS).

By following these steps, you will have a secure local environment (TLS) for gRPC testing in .NET without relying on production certificates.