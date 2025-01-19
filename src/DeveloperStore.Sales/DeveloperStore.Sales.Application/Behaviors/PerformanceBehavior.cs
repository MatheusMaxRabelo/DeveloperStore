using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DeveloperStore.Sales.Application.Behaviors;

/// <summary>
/// Performance Behavior
/// </summary>
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private const string TRACE_ID_NAME = "TraceId";

    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = new Stopwatch();

        timer.Start();
        var response = await next();
        timer.Stop();

        var elapseMilliseconds = timer.ElapsedMilliseconds;

        _logger.LogInformation($"Performance {typeof(TRequest).Name}, {TRACE_ID_NAME}: {Activity.Current?.Id}, Duration: {elapseMilliseconds}ms");

        return response;
    }
}