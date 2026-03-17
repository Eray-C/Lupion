using Microsoft.AspNetCore.Mvc;

namespace Lupion.MVC.Controllers;

public class BaseController : Controller
{
    [HttpGet("GetPartialView")]

    public PartialViewResult GetPartialView(string name)
    {
        return PartialView(name);
    }
}