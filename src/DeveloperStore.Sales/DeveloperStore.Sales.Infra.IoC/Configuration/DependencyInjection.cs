using DeveloperStore.Sales.Application.Commands.Sales.Create;
using DeveloperStore.Sales.Application.Services;
using DeveloperStore.Sales.Domain.Interfaces.Repositories;
using DeveloperStore.Sales.Domain.Interfaces.Services;
using DeveloperStore.Sales.Domain.RabbitMQ;
using DeveloperStore.Sales.Infra.Data.Context;
using DeveloperStore.Sales.Infra.Data.Repositories;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DeveloperStore.Sales.Infra.IoC.Configuration;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        AddServicesDependencyInjection(services);
        AddRepositoriesDependencyInjection(services);
        RabbitMQConfig(services, configuration);
        AddValidations(services);
        services.AddDbContext<SalesContext>();
    }

    private static void AddValidations(IServiceCollection services)
    {
        services.AddSingleton<IValidator<CreateSaleCommand>, CreateSaleValidation>();
        services.AddSingleton<IValidator<ProductRequest>, ProductRequestValidator>();
    }

    private static void AddRepositoriesDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<ISalesRepository, SaleRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
    }

    private static void AddServicesDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<ISalesService, SalesService>();
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
