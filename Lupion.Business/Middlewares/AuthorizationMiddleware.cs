using Empty_ERP_Template.Business.Helpers;
using Empty_ERP_Template.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Empty_ERP_Template.Business.Middlewares;

public class AuthorizationMiddleware(RequestDelegate next)
{
  
    public async Task InvokeAsync(HttpContext context)
    {
        var path = StringHelper.NormalizePath((context.Request.Path.Value ?? "").TrimEnd('/').ToLowerInvariant());
        

          var userService = context.RequestServices.GetRequiredService<UserService>();
        var method = context.Request.Method.ToUpperInvariant();
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var permission =await userService.GetRequiredModuleOperationIdAsync(path, method);
        if (permission == null)
        {
            await next(context);
            return;
        }

        var isApiRequest = (context.Request.Path.Value ?? "").StartsWith("/api", StringComparison.OrdinalIgnoreCase);

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            if (isApiRequest)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
            else
                context.Response.Redirect("/Login/Unauthorized");
            return;
        }

        var userRolePermissions = await userService.GetUserRolePermissionsAsync(userId);
        if (!userRolePermissions.Contains(permission ?? default))
        {
            if (isApiRequest)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden");
            }
            else
                context.Response.Redirect("/Login/Unauthorized");
            return;
        }

        await next(context);
    }

   
}
