using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Lupion.API.ActionFilters;

public class PutRequestValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!HttpMethods.IsPut(context.HttpContext.Request.Method))
        {
            await next();
            return;
        }

        var routeIdKvp = context.ActionArguments
            .FirstOrDefault(kv => string.Equals(kv.Key, "id", StringComparison.OrdinalIgnoreCase));

        if (routeIdKvp.Equals(default(KeyValuePair<string, object?>)) || routeIdKvp.Value is null)
        {
            await next();
            return;
        }

        if (!ActionHasBodyParameter(context))
        {
            await next();
            return;
        }

        var bodyArg = context.ActionArguments
            .FirstOrDefault(kv => kv.Value is not null && IsComplexType(kv.Value.GetType()));

        if (bodyArg.Equals(default(KeyValuePair<string, object?>)) || bodyArg.Value is null)
        {
            context.Result = new BadRequestObjectResult(new { Message = "Request eksik." });
            return;
        }

        // Body içinde Id propertyâ€™si (case-insensitive)
        var idProp = bodyArg.Value.GetType()
            .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (idProp is null)
        {
            context.Result = new BadRequestObjectResult(new { Message = "Request içinde Id bulunamadı." });
            return;
        }

        var routeIdStr = NormalizeId(routeIdKvp.Value);
        var bodyIdStr = NormalizeId(idProp.GetValue(bodyArg.Value));

        if (routeIdStr is null || bodyIdStr is null || !string.Equals(routeIdStr, bodyIdStr, StringComparison.Ordinal))
        {
            context.Result = new BadRequestObjectResult(new { Message = "URL id ile body içindeki Id uyuşmuyor." });
            return;
        }

        await next();
    }

    private static bool ActionHasBodyParameter(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor descriptor)
            return false;
        var method = descriptor.MethodInfo;
        return method.GetParameters().Any(p => IsComplexType(p.ParameterType));
    }

    private static bool IsComplexType(Type t)
        => !(t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(Guid));

    private static string? NormalizeId(object? value)
    {
        if (value is null) return null;
        return value switch
        {
            int i => i.ToString(),
            long l => l.ToString(),
            Guid g => g.ToString("D"),
            string s => s.Trim(),
            _ => value.ToString()
        };
    }
}
