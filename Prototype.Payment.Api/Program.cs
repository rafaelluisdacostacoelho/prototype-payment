using Prototype.Payment.Application.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CreditCardsApplication>();

app.Run();

[JsonSerializable(typeof(object[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
