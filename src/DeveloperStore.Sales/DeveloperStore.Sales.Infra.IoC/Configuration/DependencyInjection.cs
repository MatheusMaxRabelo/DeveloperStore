using DeveloperStore.Sales.Infra.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.Infra.IoC.Configuration;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesContext>();
    }
}
