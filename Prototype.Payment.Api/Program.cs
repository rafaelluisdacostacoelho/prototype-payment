using Prototype.Payment.Api.Procedures;
using Prototype.Payment.Application;

var builder = WebApplication.CreateSlimBuilder(args);

// Adiciona os servi�os via m�todo de extens�o
builder.Services.AddApplicationServices();

// Adiciona os servi�os gRPC
builder.Services.AddGrpc();

var app = builder.Build();

// Configura os servi�os gRPC
app.MapGrpcService<CreditCardGrpcService>();

// Exposi��o dos endpoints REST
app.MapControllers();

app.Run();
