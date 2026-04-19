using PurchaseTransactionAPI.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace PurchaseTransactionAPI.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate _next,
        ILogger<ExceptionHandlingMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            CurrencyNotSupportedException => HttpStatusCode.BadRequest,
            ExchangeRateNotFoundException => HttpStatusCode.NotFound,
            ExternalServiceException => HttpStatusCode.BadGateway,

            ArgumentException => HttpStatusCode.BadRequest,

            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            message = exception.Message,
            statusCode = context.Response.StatusCode,
#if DEBUG
            detail = exception.StackTrace
#endif
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}
