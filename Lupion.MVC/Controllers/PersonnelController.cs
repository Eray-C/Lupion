using Lupion.Business.Common;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.MVC.Controllers;

[Route("personnel")]
public class PersonnelController(SharedService sharedService) : BaseController
{
    public IActionResult Index()
    {
        return View();
    }


    [HttpGet("add-edit")]
    public async Task<IActionResult> frmAddEditPersonnel()
    {
        var genderTypes = await sharedService.QueryTypesAsync("GenderType");
        var personnelStatusTypes = await sharedService.QueryTypesAsync("PersonnelStatusType");
        var departmentTypes = await sharedService.QueryTypesAsync("DepartmentType");
        var personnelTypes = await sharedService.QueryTypesAsync("PersonnelType");
        var maritalStatusTypes = await sharedService.QueryTypesAsync("MaritalStatusType");

        return View((genderTypes, personnelStatusTypes, departmentTypes, personnelTypes, maritalStatusTypes));
    }

    [HttpGet("license/add-edit")]
    public async Task<IActionResult> frmAddEditLicenseCerticates()
    {
        var licenceTypes = await sharedService.QueryTypesAsync("LicenseType");

        return View("LicensesCertificates/frmAddEditLicenseCerticates", licenceTypes);
    }

    [HttpGet("salary/add-edit")]
    public async Task<IActionResult> frmAddEditSalary()
    {
        var currencies = await sharedService.GetCurrencies();
        var paymentTypes = await sharedService.QueryTypesAsync("PersonnelPaymentType");

        return View(
            "Salary/frmAddEditSalary",
            (currencies, paymentTypes)
        );
    }

    [HttpGet("bonus/add-edit")]
    public async Task<IActionResult> frmAddEditBonus()
    {
        var bonusTypes = await sharedService.QueryTypesAsync("PersonnelBonusType");
        var months = TurkishMonthHelper.GetAllMonths();
        var currencies = await sharedService.GetCurrencies();
        return View("Bonus/frmAddEditBonus", (bonusTypes, months, currencies));
    }

    [HttpGet("deduction/add-edit")]
    public async Task<IActionResult> frmAddEditDeduction()
    {
        var deductionTypes = await sharedService.QueryTypesAsync("PersonnelDeductionType");
        var months = TurkishMonthHelper.GetAllMonths();
        var currencies = await sharedService.GetCurrencies();
        return View("Deduction/frmAddEditDeduction", (deductionTypes, months, currencies));
    }

    [HttpGet("advance/add-edit")]
    public async Task<IActionResult> frmAddEditAdvance()
    {
        var currencies = await sharedService.GetCurrencies();
        return View("Advance/frmAddEditAdvance", currencies);
    }

    [HttpGet("contact/add-edit")]
    public IActionResult frmAddEditContact()
    {
        return View("Contact/frmAddEditContact");
    }

    [HttpGet("relative-contact/add-edit")]
    public async Task<IActionResult> frmAddEditRelativeContact()
    {
        var relationshipTypes = await sharedService.QueryTypesAsync("RelationshipType");
        var genderTypes = await sharedService.QueryTypesAsync("GenderType");

        return View("Contact/frmAddEditRelativeContact", (relationshipTypes, genderTypes));
    }
    [HttpGet("payroll")]
    public IActionResult Payroll()
    {
        return View("Payroll/Index");
    }

    [HttpGet("payroll/period-modal")]
    public IActionResult frmPayrollPeriod()
    {
        return PartialView("Payroll/frmPayrollPeriod");
    }

    [HttpGet("payroll/realize-modal")]
    public IActionResult frmRealizePayroll()
    {
        return PartialView("Payroll/frmRealizePayroll");
    }
}
