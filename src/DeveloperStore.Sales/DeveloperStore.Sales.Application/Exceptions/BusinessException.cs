using System.Diagnostics;
using DeveloperStore.Sales.Application.Response;

namespace DeveloperStore.Sales.Application.Exceptions;

public class BusinessException : Exception
{
    public string? TraceId { get; set; }
    public string? Code { get; set; } = null!;
    public ErrorResponse Error { get; set; }

    public BusinessException(string message) : base(message)
    {
        TraceId = Activity.Current?.Id;
    }

    public BusinessException(ErrorResponse error, params ErrorResponse[] errors) : base()
    {
        Error = error;
    }
}
