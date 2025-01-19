using DeveloperStore.Sales.Domain.RabbitMQ;
using DeveloperStore.Sales.Infra.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DeveloperStore.Sales.Infra.IoC.Configuration;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesContext>();

        RabbitMQConfig(services, configuration);
    }

    private static void RabbitMQConfig(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMQConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["Bus:HostName"]
            };

            if (!string.IsNullOrEmpty(configuration["Bus:UserName"]))
            {
                factory.UserName = configuration["Bus:UserName"];
            }

            if (!string.IsNullOrEmpty(configuration["Bus:Password"]))
            {
                factory.Password = configuration["Bus:Password"];
            }

            return new RabbitMQConnection(factory);
        });
    }
}
