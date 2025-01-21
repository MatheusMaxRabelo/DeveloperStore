using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DeveloperStore.Sales.Application.Behaviors;

/// <summary>
/// Logging Behavior
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private const string SEPARATOR = ", ";
    private const string TRACE_ID_NAME = "TraceId";

    private readonly ILogger _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestPropertiesDictionary = BuildDictionaryProperties(request);

        _logger.LogInformation($"Handling {typeof(TRequest).Name}, {TRACE_ID_NAME}: {Activity.Current?.Id}, {JoinPropertiesToMessage(requestPropertiesDictionary)}");

        var response = await next();

        var responsePropertiesDictionary = BuildDictionaryProperties(response);

        _logger.LogInformation($"Handled {typeof(TResponse).Name}, {TRACE_ID_NAME}: {Activity.Current?.Id}, {JoinPropertiesToMessage(responsePropertiesDictionary)}");

        return response;
    }

    /// <summary>
    /// Build dictionary properties
    /// </summary>
    /// <param name="obj">Object</param>
    /// <returns>Properties in dictionary</returns>
    private IDictionary<string, string> BuildDictionaryProperties(object? obj)
    {
        var dictionary = new Dictionary<string, string>();

        if (obj is null)
            return dictionary;

        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(obj, null)?.ToString() ?? string.Empty;
            dictionary.Add(property.Name, value);
        }

        return dictionary;
    }

    /// <summary>
    /// Join properties in string
    /// </summary>
    /// <param name="properties">Properties</param>
    /// <returns>String properties with separator</returns>
    private string JoinPropertiesToMessage(IDictionary<string, string> properties)
        => string.Join(SEPARATOR, properties.Select(p => $"{p.Key}: {p.Value}"));
}