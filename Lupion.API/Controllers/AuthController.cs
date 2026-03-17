using Lupion.Business.Requests.Authentication;
using Lupion.Business.Responses;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(AuthService authService) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        var token = await authService.LoginAsync(loginRequest);
        var response = new LoginResponse { Token = token };
        return Ok(response, "GiriÅŸ iÅŸlemi baÅŸarÄ±lÄ±");
    }

    [HttpPost("logout")]
    public IActionResult Logout() => Ok("Ã‡Ä±kÄ±ÅŸ iÅŸlemi baÅŸarÄ±lÄ±");

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest registerRequest)
    {
        await authService.RegisterAsync(registerRequest);

        return Ok("KayÄ±t baÅŸarÄ±lÄ±");
    }
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] RegisterRequest registerRequest)
    {
        await authService.UpdateAsync(id, registerRequest);

        return Ok("KayÄ±t baÅŸarÄ±lÄ±");
    }
}
