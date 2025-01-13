using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Prototype.Payment.Api.Services;
using Prototype.Payment.Application.Extensions;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5136, listenOptions =>
    {
        var certificate = new X509Certificate2("Certificates/Server.pfx", "Reb0rn777");
        var httpHandler = new HttpClientHandler();

        httpHandler.ClientCertificates.Add(certificate);

        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps(certificate, httpsOptions =>
        {
            httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
        });
    });
});

// Add application services through extension method
builder.Services.AddApplicationServices();

// Add gRPC
builder.Services.AddGrpc();

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure gRPC services
app.MapGrpcService<CreditCardService>();

// Expose REST endpoints
app.MapControllers();

app.Run();
