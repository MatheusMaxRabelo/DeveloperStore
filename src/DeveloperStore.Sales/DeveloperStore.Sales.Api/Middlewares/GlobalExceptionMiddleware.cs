using DeveloperStore.Sales.Application.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Text.Json;
using FluentValidation;
using DeveloperStore.Sales.Application.Response;

namespace DeveloperStore.Sales.Api.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    #region Constants
    private const string CONTENT_TYPE = "application/json";
    private const string TRACE_ID_NAME = "TraceId";
    private const string ENDPOINT_NAME = "Endpoint";
    private const string QTY_OF_ERROR = "Total of errors";
    private const string NOT_FOUND = "0002";
    #endregion

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);

        }
        catch (ValidationException e)
        {

            var response = new List<ErrorResponse>();

            response.AddRange(
                e.Errors.Select(error => new ErrorResponse
                {
                    Type = error.ErrorCode,
                    Error = error.PropertyName,
                    Detail = error.ErrorMessage
                })
            );

            logger.LogError(e, $"{QTY_OF_ERROR} : {response.Count},{ENDPOINT_NAME}: {context.Request.Path}, {TRACE_ID_NAME}");

            await WriteResponseAsync(context, HttpStatusCode.BadRequest, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
        catch (BusinessException e)
        {
            var response = e.Error;

            logger.LogError(e, $"Business failure: {e.Message},  {ENDPOINT_NAME}: {context.Request.Path}, {TRACE_ID_NAME}: {e.TraceId}");

            if (e.Code != null && e.Code[4..e.Code.Length].Equals(NOT_FOUND))
                await WriteResponseAsync(context, HttpStatusCode.NotFound, JsonSerializer.Serialize(response), CONTENT_TYPE);

            else
                await WriteResponseAsync(context, HttpStatusCode.BadRequest, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
        catch (Exception e)
        {
            var response = new ErrorResponse() { Type = e.GetType().Name, Error = e.Message, Detail = e.InnerException.Message };

            logger.LogError(e, $"{ENDPOINT_NAME}: {context.Request.Path}, {TRACE_ID_NAME}");

            await WriteResponseAsync(context, HttpStatusCode.BadRequest, JsonSerializer.Serialize(response), CONTENT_TYPE);
        }
    }
    private async Task WriteResponseAsync(HttpContext context, HttpStatusCode httpResponseStatusCode, string body, string contentType)
    {
        context.Response.Clear();
        context.Response.StatusCode = (int)httpResponseStatusCode;
        context.Response.ContentType = contentType;
        await context.Response.WriteAsync(body);
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}