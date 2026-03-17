using Lupion.Business.Requests.Shared;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[ApiController]
[Route("api/shared")]
public class SharedController(SharedService sharedService) : BaseController
{
    [HttpGet("types/{category}/{parentId?}")]
    public async Task<IActionResult> QueryTypesAsync(string category, int? parentId = null)
    {
        var vehicles = await sharedService.QueryTypesAsync(category, parentId);
        return Ok(vehicles);
    }


    [HttpPost("types")]
    public async Task<IActionResult> AddTypeAsync([FromBody] GeneralTypeRequest request)
    {
        var typeId = await sharedService.AddTypeAsync(request);

        return Ok(typeId);
    }
    [HttpPut("types/{id}")]
    public async Task<IActionResult> UpdateTypeAsync(int id, [FromBody] GeneralTypeRequest request)
    {
        await sharedService.UpdateTypeAsync(id, request);
        return Ok();
    }

    [HttpDelete("types/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await sharedService.DeleteTypeAsync(id);

        return Ok();
    }

    [HttpGet("currencies")]
    public async Task<IActionResult> GetCurrencies()
    {
        var currencies = await sharedService.GetCurrencies();

        return Ok(currencies);
    }
}
