using Lupion.Business.Helpers;
using Lupion.Data;
using Lupion.Data.Entities;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace Lupion.API.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger, IServiceScopeFactory scopeFactory)
{
    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Request.EnableBuffering();
        string requestBody = "";
        if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);

        stopwatch.Stop();

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var user = context.User;

        string? userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? companyCode = user?.FindFirst("CompanyCode")?.Value;
        string? userFullName = $"{user?.FindFirst("FirstName")?.Value} {user?.FindFirst("LastName")?.Value}";

        var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim()
            ?? context.Connection.RemoteIpAddress?.ToString();

        var log = new HttpLog
        {
            UserId = userId,
            UserFullName = userFullName,
            CompanyCode = companyCode,
            ClientIp = clientIp,
            Method = context.Request.Method,
            Endpoint = $"{context.Request.Path}{context.Request.QueryString}",
            StatusCode = context.Response.StatusCode,
            RequestBody = requestBody,
            ResponseBody = responseText,
            DurationMs = stopwatch.ElapsedMilliseconds,
            CurlCommand = CurlHelper.GenerateCurl(context.Request, requestBody),
        };

        try
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ManagementDBContext>();
            db.Set<HttpLog>().Add(log);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Log insert failed");
        }

        await responseBody.CopyToAsync(originalBodyStream);
    }
}

