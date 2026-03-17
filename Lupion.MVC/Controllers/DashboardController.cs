using Lupion.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lupion.MVC.Controllers;

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
