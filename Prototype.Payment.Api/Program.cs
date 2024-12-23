using Prototype.Payment.Api.Procedures;
using Prototype.Payment.Application;

var builder = WebApplication.CreateSlimBuilder(args);

// Adiciona os serviços via método de extensão
builder.Services.AddApplicationServices();

// Adiciona os serviços gRPC
builder.Services.AddGrpc();

var app = builder.Build();

// Configura os serviços gRPC
app.MapGrpcService<CreditCardGrpcService>();

// Exposição dos endpoints REST
app.MapControllers();

app.Run();
