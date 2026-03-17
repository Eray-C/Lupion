using Empty_ERP_Template.Business.Exceptions;
using Empty_ERP_Template.Business.Requests.Authentication;
using Empty_ERP_Template.Business.Responses;
using Empty_ERP_Template.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.MVC.Controllers;

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
                Message = "Giriş işlemi başarılı"
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
            return Ok(new BaseResponse { Success = true, Message = "Şifre sıfırlama e-postası gönderildi" });
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
            return Ok(new BaseResponse { Success = true, Message = "Şifre güncellendi" });
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
            Message = "Çıkış işlemi başarılı"
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
