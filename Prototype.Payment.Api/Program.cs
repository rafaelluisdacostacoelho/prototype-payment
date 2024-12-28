using Microsoft.AspNetCore.Server.Kestrel.Core;
using Prototype.Payment.Api.Services;
using Prototype.Payment.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5166, o => o.Protocols = HttpProtocols.Http1);
    options.ListenAnyIP(5136, o => o.Protocols = HttpProtocols.Http2);
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
