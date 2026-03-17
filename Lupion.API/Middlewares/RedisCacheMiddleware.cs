using Empty_ERP_Template.Business.Attributes;
using Empty_ERP_Template.Business.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
public class RedisCacheMiddleware
{
    private readonly RequestDelegate _next;

    public RedisCacheMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, RedisCacheService cacheService)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        var method = descriptor?.MethodInfo;
        var cacheAttr = method?.GetCustomAttribute<RedisCacheAttribute>();

        if (cacheAttr == null)
        {
            await _next(context);
            return;
        }

        var cacheKey = $"ApiCache:{cacheAttr.Key}";
        var cached = await cacheService.GetAsync<string>(cacheKey);

        if (!string.IsNullOrEmpty(cached))
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(cached);
            return;
        }

        var originalBodyStream = context.Response.Body;
        await using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await _next(context); // controller'ý çalýþtýr
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            // ? Client’a yaz
            memoryStream.Seek(0, SeekOrigin.Begin);
            context.Response.ContentLength = null; // body deðiþtiði için zorunlu
            await memoryStream.CopyToAsync(originalBodyStream);

            // ? Cache’e kaydet
            if (context.Response.StatusCode == 200 && !string.IsNullOrWhiteSpace(responseBody))
            {
                var expiry = cacheAttr.DurationMinutes.HasValue
                    ? TimeSpan.FromMinutes(cacheAttr.DurationMinutes.Value)
                    : (TimeSpan?)null;

                await cacheService.SetAsync(cacheKey, responseBody, expiry);

                foreach (var entity in cacheAttr.EntityNames)
                    await cacheService.RegisterKeyForEntityAsync(entity, cacheKey);
            }
        }
        finally
        {
            context.Response.Body = originalBodyStream; // ?? mutlaka geri al
        }
    }
}
