using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.MVC.Controllers;

public class BaseController : Controller
{
    [HttpGet("GetPartialView")]

    public PartialViewResult GetPartialView(string name)
    {
        return PartialView(name);
    }
}