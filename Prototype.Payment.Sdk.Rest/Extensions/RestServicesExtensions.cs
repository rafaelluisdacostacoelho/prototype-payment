using Microsoft.Extensions.DependencyInjection;
using Prototype.Payment.Sdk.Rest.Configurations;
using RestEase;

namespace Prototype.Payment.Sdk.Rest.Extensions;

public static class RestServicesExtensions
{
    public static IServiceCollection AddRestClient(this IServiceCollection services, PaymentApiSettings settings)
    {
        services.AddSingleton(provider => RestClient.For<ICreditCard>(settings?.ApiEndpoint));

        return services;
    }
}
