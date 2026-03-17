using Empty_ERP_Template.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Empty_ERP_Template.MVC.Controllers;

public class DashboardController() : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
