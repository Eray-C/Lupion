using System.Net;
using System.Text.Json;

namespace Lupion.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is ObjectDisposedException || context.Response.HasStarted)
            {
                _logger.LogWarning(ex, "Response stream kapandÄ±, exception handle edildi.");
                return;
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Global exception yakalandÄ±.");

        var (statusCode, message) = GetStatusCodeAndMessage(ex);

        var response = new
        {
            message = message,
            error = ex.GetType().Name
        };

        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var json = JsonSerializer.Serialize(response, _jsonOptions);
            await context.Response.WriteAsync(json);
        }
    }

    private (int StatusCode, string Message) GetStatusCodeAndMessage(Exception ex)
    {
        return ex switch
        {
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Yetkisiz eriÅŸim."),
            FileNotFoundException => ((int)HttpStatusCode.NotFound, "Dosya bulunamadÄ±."),
            Minio.Exceptions.MinioException => ((int)HttpStatusCode.BadRequest, "Dosya yÃ¼klenemedi."),
            InvalidOperationException => ((int)HttpStatusCode.BadRequest, ex.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Beklenmeyen hata oluÅŸtu.")
        };
    }
}
