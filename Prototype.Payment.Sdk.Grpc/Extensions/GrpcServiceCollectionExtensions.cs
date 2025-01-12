using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace Prototype.Payment.Sdk.Grpc.Extensions;

public static class GrpcServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcClient(this IServiceCollection services)
    {
        var certificate = new X509Certificate2("Certificates/Client.pfx", "ClientPassword");

        var httpHandler = new HttpClientHandler();
        
        httpHandler.ClientCertificates.Add(certificate);

        var grpcChannelOptions = new GrpcChannelOptions
        { 
            HttpHandler = httpHandler
        };

        using var grpcChannel = GrpcChannel.ForAddress("", grpcChannelOptions);

        services.AddSingleton(grpcChannel);

        services.AddScoped(serviceProvider =>
        {
            var channel = serviceProvider.GetRequiredService<GrpcChannel>();
            return new CreditCardGrpcService.CreditCardGrpcServiceClient(channel);
        });

        return services;
    }
}