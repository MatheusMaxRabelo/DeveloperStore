using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;

namespace DeveloperStore.Sales.Application.Exceptions;

public class BusinessException : Exception
{
    public string? TraceId { get; set; }
    public List<Error> Errors { get; set; } = new List<Error>();

    public BusinessException(string message) : base(message)
    {
        TraceId = Activity.Current?.Id;
    }

    public BusinessException(Error error, params Error[] errors) : base()
    {
        Errors.Add(error);

        if (errors.Count() > default(int))
            Errors.AddRange(errors);
    }
    public BusinessException(string message, List<Error> errors) : base(message)
    {
        TraceId = Activity.Current?.Id;
        Errors = errors;
    }

    public BusinessException(string? message, Exception? innerException) : base(message, innerException)
    {
        TraceId = Activity.Current?.Id;
    }
}
