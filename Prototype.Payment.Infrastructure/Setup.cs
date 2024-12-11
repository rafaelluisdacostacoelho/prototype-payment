using Microsoft.Extensions.DependencyInjection;
using Prototype.Domain.Repositories;
using Prototype.Infrastructure.Repositories;

namespace Prototype.Payment.Infrastructure;

public static class Setup
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ICreditCardsRepository, InMemoryCreditCardRepository>();
    }
}
