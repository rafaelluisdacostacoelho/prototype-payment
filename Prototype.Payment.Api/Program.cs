using Prototype.Payment.Api.Services;
using Prototype.Payment.Application;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);

    serverOptions.Limits.Http2.MaxStreamsPerConnection = 100;
});

// Adiciona os servi�os via m�todo de extens�o
builder.Services.AddApplicationServices();

// Adiciona os servi�os gRPC
builder.Services.AddGrpc();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

// Configura os servi�os gRPC
app.MapGrpcService<CreditCardService>();

// Exposi��o dos endpoints REST
app.MapControllers();

app.Run();
