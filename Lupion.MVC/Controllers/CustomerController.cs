using Microsoft.AspNetCore.Mvc;

namespace Lupion.MVC.Controllers;

[Route("customer")]
public class CustomerController() : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("add-edit")]
    public IActionResult frmAddEditCustomer()
    {
        return View();
    }
    [HttpGet("price/add-edit")]
    public IActionResult frmAddEditPrice()
    {
        return View("Prices/frmAddEditPrice");
    }
}

