using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Lupion.MVC.ActionFilter;

public class JwtUserViewBagFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.Controller as Controller;
        if (controller == null) return;

        var request = context.HttpContext.Request;
        var token = request.Cookies["access_token"];

        if (string.IsNullOrWhiteSpace(token)) return;

        var handler = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = handler.ReadJwtToken(token);

            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var firstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            var lastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;

            controller.ViewBag.User = new
            {
                Id = id,
                UserName = userName,
                Email = email,
                Role = role,
                FirstName = firstName,
                LastName = lastName,
            };
        }
        catch
        {
            // Hatalı veya geçersiz token varsa sessizce geç
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Gerek yok
    }
}
