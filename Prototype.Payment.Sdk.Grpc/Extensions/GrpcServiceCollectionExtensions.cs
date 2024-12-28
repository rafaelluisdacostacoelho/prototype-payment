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

        // Configurar o canal do cliente
        services.AddSingleton(GrpcChannel.ForAddress(address, new GrpcChannelOptions { HttpHandler = httpHandler }));

        // Registrar o cliente gRPC para injeção de dependência
        // Aqui a injeção do cliente é feita através do canal
        services.AddScoped(serviceProvider =>
        {
            var channel = serviceProvider.GetRequiredService<GrpcChannel>(); // Obtém o canal gRPC do DI
            return new CreditCardGrpcService.CreditCardGrpcServiceClient(channel); // Cria o cliente passando o canal
        });

        return services;
    }
}