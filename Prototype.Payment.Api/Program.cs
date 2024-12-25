using Prototype.Payment.Api.Services;
using Prototype.Payment.Application;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1);
    serverOptions.Limits.Http2.MaxStreamsPerConnection = 100;
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
