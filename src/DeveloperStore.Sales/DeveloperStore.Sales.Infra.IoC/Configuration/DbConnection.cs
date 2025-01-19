using DeveloperStore.Sales.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.Infra.IoC.Configuration;

public static class DbConnection
{
    public static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionDb = configuration.GetConnectionString("Sales");

        services.AddDbContext<SalesContext>(opt =>
        {
            opt.UseNpgsql(connectionDb);
        });
    }
}
