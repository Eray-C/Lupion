using Empty_ERP_Template.Business.Services;
using Empty_ERP_Template.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Empty_ERP_Template.MVC.Controllers;

public class CacheController(CacheService cacheService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> ClearAsync(string k)
    {
        await cacheService.ClearAsync(k);
        return Ok();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
