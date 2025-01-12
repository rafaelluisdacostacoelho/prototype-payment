# Prototype Payment

Este projeto implementa interfaces REST e gRPC.

# 1. Configuração do Ambiente de Desenvolvimento

Precisamos configurar o NuGet e o OpenSSH, este último vamos usar para criar uma Autoridade Certificadora que usaremos para gerenciar Certificados, isso é necessário para utilizarmos o gRPC de forma consistente e similar a um ambiente produtivo.
## 1.1. OpenSSH

A configuração de gRPC com TLS em ambiente local (para desenvolvimento) normalmente envolve a geração de certificados autoassinados (self-signed). A ideia geral é:

* **Criar um CA (Certificate Authority) raiz local**.
* **Criar os certificados de servidor** e assiná-los usando esse CA raiz.
* *(Opcional) **Criar certificados de cliente** para mutual TLS (se quiser autenticar o cliente também).
* **Configurar** o servidor gRPC (Kestrel, por exemplo) para usar o certificado do servidor.
* **Configurar** o cliente gRPC para confiar no CA (ou usar certificado de cliente, caso seja mTLS).
### 1.1.1. Criar um CA (Certificate Authority) raiz

O CA raiz será usado para assinar os certificados que usaremos no servidor (e no cliente se necessário). Ele terá um par de chaves (privada e pública) e um certificado. É importante **proteger** a chave privada do CA, pois ela permite assinar certificados em nome da sua “autoridade”.

```sh
\# Cria o par de chaves e o certificado para o CA raiz (autoassinado):

openssl req -x509 -nodes -newkey rsa:2048 \
	-keyout RootCA.key \ 
    -out RootCA.crt \ 
    -days 365 \ 
    -subj "/C=BR/ST=SP/L=SaoPaulo/O=MinhaEmpresa/CN=DevRootCA"
```

- `-x509` indica que estamos gerando um certificado (não só o CSR).
- `-nodes` faz com que a chave privada não seja criptografada com senha (para dev isso é comum, mas em produção, normalmente se protege com senha).
- `-subj` define os campos básicos do certificado, como país (`C`), estado (`ST`), localidade (`L`), organização (`O`) e o **Common Name** (`CN`).

Após esse comando, teremos dois arquivos principais:

- `RootCA.key`: chave privada do CA (guarde em local seguro).
- `RootCA.crt`: certificado do CA que pode ser distribuído.

### 1.1.2. Gerar o certificado do servidor

Vamos criar primeiro uma “solicitação de assinatura de certificado” (CSR – Certificate Signing Request), depois usar o CA para assinar essa solicitação.

#### 1.1.2.1. Criar chave privada e CSR

```sh
# 1) Gera a chave privada do servidor e a CSR:
openssl req -newkey rsa:2048 -nodes \ 
	-keyout Server.key \ 
	-out Server.csr \ 
	-subj "/C=BR/ST=SP/L=SaoPaulo/O=MinhaEmpresa/CN=localhost"
```

O `CN=localhost` é importante para desenvolvimento local, pois vamos acessar o servidor via `https://localhost`. Em produção, obviamente, você precisaria usar o domínio real do seu serviço.

#### 1.1.2.2. Assinar a CSR com o CA

```sh
# 2) Assina a CSR do servidor com o nosso CA raiz, gerando o certificado 
openssl x509 -req -in Server.csr \ 
	-CA RootCA.crt \ 
	-CAkey RootCA.key \ 
	-CAcreateserial \ 
	-out server.crt \ 
	-days 365
```

Esse comando gera o arquivo `Server.crt`, que é o certificado do servidor assinado pela nossa CA raiz.

> O parâmetro `-CAcreateserial` cria um arquivo `RootCA.srl` (para controle de número de série dos certificados). Em um ambiente real, você pode querer controlar esse serial manualmente.

## 1.1.3. Criar arquivo PFX (opcional, mas comum no .NET)

Para uso no .NET, é **muito comum** consolidar a chave privada e o certificado em um arquivo `PFX` (PKCS#12). Assim, o Kestrel consegue carregar o certificado e a chave em um único arquivo.

```sh
openssl pkcs12 -export \ 
	-out Server.pfx \ 
	-inkey Server.key \ 
	-in Server.crt \ 
	-certfile RootCA.crt \ 
	-passout pass:MinhaSenhaSegura
```

Isto pedirá a senha (definida em `-passout`).

- `Server.pfx`: contém o certificado do servidor + chave privada + certificado da CA no “chain”.

Em um **cenário de dev**, às vezes usamos `-passout pass:` (senha em branco) para não digitar repetidamente. Mas se possível, utilize uma senha.

## 1.1.4. Configurar o servidor gRPC (Kestrel) no .NET

### 1.1.4.1. Em `Program.cs` (ou no `Startup.cs`)

No ASP.NET Core 5+, o Kestrel pode ser configurado mais ou menos assim:

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
                    // Carrega o certificado do .pfx
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

> Note que para **desenvolvimento**, você pode também configurar via `appsettings.json`, mas em geral é comum ser feito no código (ou via secrets).
### 1.1.4.2. Configurar o cliente gRPC

A configuração do cliente vai depender do tipo de autenticação TLS que você deseja:
### 1.1.4.3. Só validar o certificado do servidor

Neste caso, basta **confiar** na CA raiz no sistema operacional. Em ambiente de desenvolvimento, você pode instalar o `rootCA.crt` localmente para que o Windows/macOS/Linux reconheçam o certificado como confiável.

- Windows: basta abrir o `rootCA.crt`, clicar em instalar, colocar na “Autoridades de Certificação Raiz Confiáveis”.
- macOS/Linux: usar as ferramentas nativas (`security add-trusted-cert`, `update-ca-trust`, etc.).

Com isso, qualquer client que faça uma chamada TLS para um certificado assinado pela `DevRootCA` passará a confiar automaticamente.

No .NET (em C#), caso você esteja fazendo chamadas gRPC, pode ser algo como:

```c#
using Grpc.Net.Client;
using Grpc.Core;
using System.Net.Http;

// Usa o canal HTTPS para se conectar ao servidor gRPC
var httpHandler = new HttpClientHandler
{
    // Caso você não tenha instalado o rootCA no sistema, você pode permitir (inseguro para prod)
    // ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, chain, errors) => true
};

using var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

// Agora você pode chamar seus métodos do gRPC ...
```

> Em **desenvolvimento**, muitas vezes acabamos “ignorando” erros de certificado definindo `ServerCertificateCustomValidationCallback = ... => true`, mas não é recomendado para produção. O ideal é instalar o certificado CA e fazer a validação de verdade.
### 1.1.4.4. Mutual TLS (mTLS)

Se você também precisar que **o servidor valide** quem é o cliente, então é preciso gerar um certificado de cliente (`client.crt` e `client.key`), assiná-lo pelo CA raiz e, em seguida, usar esse certificado no cliente.

No .NET para gRPC, podemos fazer algo como:

```c#
var clientCertificate = new X509Certificate2("certs/client.pfx", "SenhaDoCliente");

var httpHandler = new HttpClientHandler();
httpHandler.ClientCertificates.Add(clientCertificate);

using var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});
```

E, do lado do servidor, podemos configurar políticas de exigência de certificado do cliente, mas isso já envolve configurações mais específicas de `ClientCertificateMode` em Kestrel.
## 1.1.5. Resumo do fluxo

1. **Gerar e guardar** o CA raiz ( `.key` + `.crt` ).
2. Para cada “papel” (servidor, cliente, etc.), gerar CSR e assinar com a CA raiz.
3. Converter em `.pfx` se for usar no .NET (opcional, mas facilita).
4. No **servidor**, configurar o Kestrel para usar o `.pfx` do servidor.
5. No **cliente**, ou confia no CA (instalando no OS) ou carrega explicitamente o CA e faz validação de certificado manual.
## 1.1.6. Recomendações extras

- Para ambiente de **produção**, use certificados emitidos por uma CA pública confiável (ex: Let’s Encrypt, Digicert, etc.).
- Se você planeja **automatizar** o processo de geração de certificados para desenvolvimento, crie scripts (`.sh` ou `.ps1`) que executam esses comandos do OpenSSL, facilitando o onboarding de novos desenvolvedores.
- Tome cuidado ao usar o mesmo CA em ambientes diferentes. Para dev local, mantenha algo que seja facilmente revogado/descartado para não comprometer a segurança.
## Conclusão

Em resumo, montar a estrutura de certificado digital para uso com gRPC em ambiente local envolve:

1. Criar um CA local.
2. Criar e assinar os certificados de servidor/cliente com esse CA.
3. Configurar o servidor (Kestrel) para usar o certificado do servidor.
4. Configurar o cliente para confiar no CA ou usar um certificado de cliente, se necessário (mTLS).

Seguindo esses passos, você terá um ambiente local seguro (TLS) para testes de gRPC no .NET, sem depender de certificados de produção.
