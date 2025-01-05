using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Prototype.Payment.Sdk.Grpc.Extensions;

public static class GrpcServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcClient(this IServiceCollection services, string address)
    {
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        services.AddSingleton(GrpcChannel.ForAddress(address, new GrpcChannelOptions { HttpHandler = httpHandler }));

        services.AddScoped(serviceProvider =>
        {
            var channel = serviceProvider.GetRequiredService<GrpcChannel>();
            return new CreditCardGrpcService.CreditCardGrpcServiceClient(channel);
        });

        return services;
    }
}