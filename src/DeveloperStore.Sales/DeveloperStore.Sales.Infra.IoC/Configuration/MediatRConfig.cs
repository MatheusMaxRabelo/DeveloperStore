using DeveloperStore.Sales.Application.Behaviors;
using DeveloperStore.Sales.Application.Commands.Sales.Create;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.Infra.IoC.Configuration;

public static class MediatRConfig
{
    public static void ConfigMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreateSaleCommand>());

        PipelineBehavior(services);
    }

    private static void PipelineBehavior(IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

}
