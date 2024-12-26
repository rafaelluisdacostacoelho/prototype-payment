using Microsoft.Extensions.DependencyInjection;
using Prototype.Payment.Infrastructure;

namespace Prototype.Payment.Application.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddInfrastructureServices();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(ApplicationExtensions).Assembly));
    }
}
