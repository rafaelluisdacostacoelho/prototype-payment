# Configurar o CA (Certificate Authority)

Pre-requisito: OpenSSL

O CA raiz será usado para assinar os certificados que usaremos no servidor (e no cliente se necessário). Ele terá um par de chaves (privada e pública) e um certificado. É importante proteger a chave privada do CA, pois ela permite assinar certificados em nome da sua “autoridade”.

```
# Cria o par de chaves e o certificado para o CA raiz (autoassinado):

openssl req -x509 -nodes -newkey rsa:2048 \
    -keyout rootCA.key \
    -out rootCA.crt \
    -days 365 \
    -subj "/C=BR/ST=SP/L=SaoPaulo/O=MinhaEmpresa/CN=DevRootCA"
```

-x509 indica que estamos gerando um certificado (não só o CSR).
-nodes faz com que a chave privada não seja criptografada com senha (para dev isso é comum, mas em produção, normalmente se protege com senha).
-subj define os campos básicos do certificado, como país (C), estado (ST), localidade (L), organização (O) e o Common Name (CN).
Após esse comando, teremos dois arquivos principais:

rootCA.key: chave privada do CA (guarde em local seguro).
rootCA.crt: certificado do CA que pode ser distribuído.

# Configurar o Nuget

Pre-requisito: NuGet

To generate a local package, use the following command in the project directory.

`dotnet pack --configuration Release`

```sh
dotnet pack Prototype.Payment.Sdk.Grpc.csproj --configuration Release
```

Push to nuget local repo

```sh
nuget push "bin\Release\Prototype.Payment.Sdk.Grpc.1.3.0.nupkg" -Source "C:\NuGetLocalRepo"
```

To install libs locally.

```sh
dotnet add package Prototype.Payment.Sdk.Grpc.CrossCutting --source "D:\05__projetos\github\prototype-payment\Prototype.Payment.Sdk.Grpc.CrossCutting\bin\Release"
```
