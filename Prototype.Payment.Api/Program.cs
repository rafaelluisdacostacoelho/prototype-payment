using Prototype.Payment.Api.Services;
using Prototype.Payment.Application;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);

    serverOptions.Limits.Http2.MaxStreamsPerConnection = 100;
});

// Adiciona os serviços via método de extensão
builder.Services.AddApplicationServices();

// Adiciona os serviços gRPC
builder.Services.AddGrpc();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

// Configura os serviços gRPC
app.MapGrpcService<CreditCardService>();

// Exposição dos endpoints REST
app.MapControllers();

app.Run();
