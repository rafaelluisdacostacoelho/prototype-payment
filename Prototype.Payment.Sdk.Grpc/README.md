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
