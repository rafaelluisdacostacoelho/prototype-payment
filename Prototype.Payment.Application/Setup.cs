using Microsoft.Extensions.DependencyInjection;
using Prototype.Payment.Infrastructure;

namespace Prototype.Payment.Application;

public static class Setup
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddInfrastructureServices();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(Setup).Assembly));
    }
}
