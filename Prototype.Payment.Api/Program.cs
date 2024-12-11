using Prototype.Payment.Application.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CreditCardsApplication>();

app.Run();
