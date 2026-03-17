using Lupion.Business.Exceptions;
using Lupion.Business.Requests.Authentication;
using Lupion.Business.Responses;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.MVC.Controllers;

public class LoginController(AuthService authService) : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public new IActionResult Unauthorized()
    {
        Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
        return View();
    }


    [HttpGet]
    public IActionResult NotFound()
    {
        Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
        return View();
    }
    [HttpGet]
    public IActionResult Forbidden()
    {
        Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        try
        {
            var token = await authService.LoginAsync(request);

            AddAccesTokenToCookies(token);

            var response = new BaseResponse<LoginResponse>
            {
                Data = new()
                {
                    Token = token
                },
                Success = true,
                Message = "GiriÅŸ iÅŸlemi baÅŸarÄ±lÄ±"
            };

            return Ok(response);
        }
        catch (LoginInProgressException ex)
        {
            return StatusCode(429, new BaseResponse
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new BaseResponse
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            await authService.GeneratePasswordResetTokenAsync(request);
            return Ok(new BaseResponse { Success = true, Message = "Åifre sÄ±fÄ±rlama e-postasÄ± gÃ¶nderildi" });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            await authService.ResetPasswordAsync(request);
            return Ok(new BaseResponse { Success = true, Message = "Åifre gÃ¼ncellendi" });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");

        var response = new BaseResponse
        {
            Success = true,
            Message = "Ã‡Ä±kÄ±ÅŸ iÅŸlemi baÅŸarÄ±lÄ±"
        };

        return Ok(response);
    }

    private void AddAccesTokenToCookies(string token)
    {
        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(8)
        });
    }

}
