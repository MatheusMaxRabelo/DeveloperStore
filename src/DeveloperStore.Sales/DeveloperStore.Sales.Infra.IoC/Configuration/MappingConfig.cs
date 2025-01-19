using DeveloperStore.Sales.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.Infra.IoC.Configuration;

public static class MappingConfig
{
    public static void AddMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SalesMapperProfile));
    }
}
