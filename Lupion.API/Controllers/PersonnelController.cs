using Lupion.Business.Requests.Personnel;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[ApiController]
[Route("api/personnel")]
public class PersonnelController(PersonnelService personnelService) : BaseController
{


    [HttpGet]
    public async Task<IActionResult> GetPersonnelsAsync()
    {
        var vehicles = await personnelService.GetPersonnelsAsync();

        return Ok(vehicles);
    }

    [HttpGet("drivers")]
    public async Task<IActionResult> GetDriversAsync()
    {
        var drivers = await personnelService.GetDriversAsync();
        return Ok(drivers);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] PersonnelRequest request)
    {
        var result = await personnelService.AddAsync(request);

        return Ok(result, "Personnel baÅŸarÄ±yla kaydedildi");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] PersonnelRequest request)
    {
        await personnelService.UpdateAsync(id, request);

        return Ok("Personnel baÅŸarÄ±yla gÃ¼ncellendi.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await personnelService.DeleteAsync(id);

        return Ok("Personnel baÅŸarÄ±yla silindi.");
    }




    [HttpGet("license/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelLicenses(int personnelId)
    {
        var response = await personnelService.GetPersonnelLicenses(personnelId);

        return Ok(response);
    }

    [HttpPost("license")]
    public async Task<IActionResult> AddLicenseAsync([FromBody] PersonnelLicenseRequest request)
    {
        var result = await personnelService.AddLicenseAsync(request);

        return Ok(result);
    }

    [HttpPut("license/{id:int}")]
    public async Task<IActionResult> UpdateLicenseAsync(int id, [FromBody] PersonnelLicenseRequest request)
    {
        await personnelService.UpdateLicenseAsync(id, request);

        return Ok();
    }

    [HttpDelete("license/{id:int}")]
    public async Task<IActionResult> DeleteLicenseAsync(int id)
    {
        await personnelService.DeleteLicenseAsync(id);

        return Ok();
    }

    [HttpGet("salary/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelSalaries(int personnelId)
    {
        var response = await personnelService.GetPersonnelSalaries(personnelId);

        return Ok(response);
    }

    [HttpPost("salary")]
    public async Task<IActionResult> AddSalaryAsync([FromBody] PersonnelSalaryRequest request)
    {
        var result = await personnelService.AddSalaryAsync(request);

        return Ok(result);
    }

    [HttpPut("salary/{id:int}")]
    public async Task<IActionResult> UpdateSalaryAsync(int id, [FromBody] PersonnelSalaryRequest request)
    {
        await personnelService.UpdateSalaryAsync(id, request);

        return Ok();
    }

    [HttpDelete("salary/{id:int}")]
    public async Task<IActionResult> DeleteSalaryAsync(int id)
    {
        await personnelService.DeleteSalaryAsync(id);

        return Ok();
    }

    [HttpGet("bonus/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelBonuses(int personnelId)
    {
        var response = await personnelService.GetPersonnelBonuses(personnelId);
        return Ok(response);
    }

    [HttpPost("bonus")]
    public async Task<IActionResult> AddBonusAsync([FromBody] PersonnelBonusRequest request)
    {
        var result = await personnelService.AddBonusAsync(request);
        return Ok(result);
    }

    [HttpPut("bonus/{id:int}")]
    public async Task<IActionResult> UpdateBonusAsync(int id, [FromBody] PersonnelBonusRequest request)
    {
        await personnelService.UpdateBonusAsync(id, request);
        return Ok();
    }

    [HttpDelete("bonus/{id:int}")]
    public async Task<IActionResult> DeleteBonusAsync(int id)
    {
        await personnelService.DeleteBonusAsync(id);
        return Ok();
    }

    [HttpGet("deduction/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelDeductions(int personnelId)
    {
        var response = await personnelService.GetPersonnelDeductions(personnelId);
        return Ok(response);
    }

    [HttpPost("deduction")]
    public async Task<IActionResult> AddDeductionAsync([FromBody] PersonnelDeductionRequest request)
    {
        var result = await personnelService.AddDeductionAsync(request);
        return Ok(result);
    }

    [HttpPut("deduction/{id:int}")]
    public async Task<IActionResult> UpdateDeductionAsync(int id, [FromBody] PersonnelDeductionRequest request)
    {
        await personnelService.UpdateDeductionAsync(id, request);
        return Ok();
    }

    [HttpDelete("deduction/{id:int}")]
    public async Task<IActionResult> DeleteDeductionAsync(int id)
    {
        await personnelService.DeleteDeductionAsync(id);
        return Ok();
    }

    [HttpGet("advance/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelAdvances(int personnelId)
    {
        var response = await personnelService.GetPersonnelAdvances(personnelId);
        return Ok(response);
    }

    [HttpPost("advance")]
    public async Task<IActionResult> AddAdvanceAsync([FromBody] PersonnelAdvanceRequest request)
    {
        var result = await personnelService.AddAdvanceAsync(request);
        return Ok(result);
    }

    [HttpPut("advance/{id:int}")]
    public async Task<IActionResult> UpdateAdvanceAsync(int id, [FromBody] PersonnelAdvanceRequest request)
    {
        await personnelService.UpdateAdvanceAsync(id, request);
        return Ok();
    }

    [HttpDelete("advance/{id:int}")]
    public async Task<IActionResult> DeleteAdvanceAsync(int id)
    {
        await personnelService.DeleteAdvanceAsync(id);
        return Ok();
    }

    [HttpPut("advance/{id:int}/close")]
    public async Task<IActionResult> CloseAdvanceAsync(int id)
    {
        await personnelService.CloseAdvanceAsync(id);
        return Ok("Avans kapatÄ±ldÄ±.");
    }

    [HttpGet("payment-history/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelPaymentHistory(int personnelId)
    {
        var response = await personnelService.GetPersonnelPaymentHistorySummaryAsync(personnelId);
        return Ok(response);
    }

    [HttpPost("advance-payment")]
    public async Task<IActionResult> RecordAdvancePayment([FromBody] PersonnelAdvancePaymentRequest request)
    {
        await personnelService.RecordAdvancePaymentAsync(request);
        return Ok("Ã–deme kaydedildi.");
    }

    [HttpGet("contact/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelContacts(int personnelId)
    {
        var response = await personnelService.GetPersonnelContacts(personnelId);
        return Ok(response);
    }

    [HttpPost("contact")]
    public async Task<IActionResult> AddContactAsync([FromBody] PersonnelContactRequest request)
    {
        var result = await personnelService.AddContactAsync(request);

        return Ok(result);
    }

    [HttpPut("contact/{id:int}")]
    public async Task<IActionResult> UpdateContactAsync(int id, [FromBody] PersonnelContactRequest request)
    {
        await personnelService.UpdateContactAsync(id, request);

        return Ok();
    }

    [HttpDelete("contact/{id:int}")]
    public async Task<IActionResult> DeleteContactAsync(int id)
    {
        await personnelService.DeleteContactAsync(id);

        return Ok();
    }


    [HttpGet("relative-contact/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelRelativeContacts(int personnelId)
    {
        var response = await personnelService.GetPersonnelRelativeContacts(personnelId);

        return Ok(response);
    }

    [HttpPost("relative-contact")]
    public async Task<IActionResult> AddRelativeContactAsync([FromBody] PersonnelRelativeContactRequest request)
    {
        var result = await personnelService.AddRelativeContactAsync(request);

        return Ok(result);
    }

    [HttpPut("relative-contact/{id:int}")]
    public async Task<IActionResult> UpdateRelativeContactAsync(int id, [FromBody] PersonnelRelativeContactRequest request)
    {
        await personnelService.UpdateRelativeContactAsync(id, request);

        return Ok();
    }

    [HttpDelete("relative-contact/{id:int}")]
    public async Task<IActionResult> DeleteRelativeContactAsync(int id)
    {
        await personnelService.DeleteRelativeContactAsync(id);

        return Ok();
    }

    [HttpGet("all-data/{personnelId:int}")]
    public async Task<IActionResult> GetPersonnelAllDataAsync(int personnelId)
    {
        var result = await personnelService.GetPersonnelAllDataAsync(personnelId);
        return Ok(result);
    }
    [HttpGet("{personnelId:int}/photo")]
    public async Task<IActionResult> GetPersonnelPhoto(int personnelId)
    {
        var base64 = await personnelService.GetPersonnelPhoto(personnelId);
        if (string.IsNullOrEmpty(base64))
            return Ok(null);

        return Ok<string>(base64);
    }

    [HttpGet("payroll/periods")]
    public async Task<IActionResult> GetPayrollPeriods()
    {
        var list = await personnelService.GetPayrollPeriodsAsync();
        return Ok(list);
    }

    [HttpGet("payroll")]
    public async Task<IActionResult> GetMonthlyPayroll([FromQuery] int year, [FromQuery] int month)
    {
        var payroll = await personnelService.GetMonthlyPayrollAsync(year, month);

        return Ok(payroll);
    }

    [HttpPost("payroll")]
    public async Task<IActionResult> SaveMonthlyPayroll([FromBody] PersonnelPayrollBatchRequest request)
    {
        await personnelService.SaveMonthlyPayrollAsync(request);

        return Ok("Bordro baÅŸarÄ±yla kaydedildi.");
    }

    [HttpPost("payroll/{payrollId:int}/realize")]
    public async Task<IActionResult> MarkPayrollAsRealized(int payrollId)
    {
        await personnelService.MarkPayrollAsRealizedAsync(payrollId);
        return Ok("Ã–deme gerÃ§ekleÅŸti olarak kaydedildi.");
    }

    [HttpDelete("payroll/period/{year:int}/{month:int}")]
    public async Task<IActionResult> DeletePayrollPeriod(int year, int month)
    {
        await personnelService.SoftDeletePeriodPayrollsAsync(year, month);
        return Ok("Bordro kayÄ±tlarÄ± silindi.");
    }

    [HttpPost("payroll/soft-delete-period")]
    public async Task<IActionResult> SoftDeletePeriodPayrolls([FromQuery] int year, [FromQuery] int month)
    {
        await personnelService.SoftDeletePeriodPayrollsAsync(year, month);
        return Ok("Bordro kayÄ±tlarÄ± silindi, sisteme tekrar hesaplatÄ±ldÄ±.");
    }

    [HttpPost("payroll/realize-all")]
    public async Task<IActionResult> MarkAllPayrollsInPeriodAsRealized([FromBody] PersonnelPayrollRealizeRequest request)
    {
        await personnelService.MarkAllPayrollsInPeriodAsRealizedAsync(request);
        return Ok("TÃ¼m Ã¶demeler gerÃ§ekleÅŸti olarak kaydedildi.");
    }
}
