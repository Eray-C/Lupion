using Lupion.Business.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

public class BaseController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Ok(string message = "")
    {
        var response = new BaseResponse
        {
            Message = message,
            Success = true
        };

        return base.Ok(response);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Ok<T>(T value)
    {
        var response = new BaseResponse<T>
        {
            Data = value,
            Success = true,
        };

        return base.Ok(response);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Ok<T>(T value, string message)
    {
        var response = new BaseResponse<T>
        {
            Data = value,
            Message = message,
            Success = true
        };

        return base.Ok(response);
    }
}