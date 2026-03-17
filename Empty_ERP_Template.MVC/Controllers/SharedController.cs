using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.MVC.Controllers;

public class SharedController() : BaseController
{
    [HttpGet]
    public IActionResult DocumentLayout()
    {
        return PartialView("_DocumentLayout");
    }
    public IActionResult GeneralTypes()
    {
        return PartialView("GeneralType/GeneralTypes");
    }
    public IActionResult frmAddGeneralTypes()
    {
        return PartialView("GeneralType/frmAddGeneralTypes");
    }
}
