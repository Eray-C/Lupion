using Lupion.Business.Requests.Customer;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController(CustomerService customerService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync()
    {
        var result = await customerService.GetCustomersAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CustomerRequest request)
    {
        var id = await customerService.AddAsync(request);
        return Ok(id, "Müşteri başarıyla kaydedildi");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerRequest request)
    {
        await customerService.UpdateAsync(id, request);
        return Ok("Müşteri başarıyla güncellendi.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await customerService.DeleteAsync(id);
        return Ok("Müşteri başarıyla silindi.");
    }

    [HttpGet("contract/{customerId:int}")]
    public async Task<IActionResult> GetContracts(int customerId)
    {
        var result = await customerService.GetContractsAsync(customerId);
        return Ok(result);
    }

    [HttpPost("contract")]
    public async Task<IActionResult> AddContract([FromBody] CustomerContractRequest request)
    {
        var id = await customerService.AddContractAsync(request);
        return Ok(id);
    }

    [HttpPut("contract/{id:int}")]
    public async Task<IActionResult> UpdateContract(int id, [FromBody] CustomerContractRequest request)
    {
        await customerService.UpdateContractAsync(id, request);
        return Ok();
    }

    [HttpDelete("contract/{id:int}")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        await customerService.DeleteContractAsync(id);
        return Ok();
    }


    [HttpGet("all-data/{customerId:int}")]
    public async Task<IActionResult> GetCustomerAllDataAsync(int customerId)
    {
        var result = await customerService.GetCustomerAllDataAsync(customerId);
        return Ok(result);
    }


    [HttpPost("price")]
    public async Task<IActionResult> AddPrice([FromBody] CustomerPriceRequest request)
    {
        var id = await customerService.AddPriceAsync(request);
        return Ok(id);
    }

    [HttpPut("price/{id:int}")]
    public async Task<IActionResult> UpdatePrice(int id, [FromBody] CustomerPriceRequest request)
    {
        await customerService.UpdatePriceAsync(id, request);
        return Ok();
    }

    [HttpDelete("price/{id:int}")]
    public async Task<IActionResult> DeletePrice(int id)
    {
        await customerService.DeletePriceAsync(id);
        return Ok();
    }

    [HttpGet("price/{customerId:int}")]
    public async Task<IActionResult> GetCustomerPrices(int customerId)
    {
        var result = await customerService.GetPricesAsync(customerId);
        return Ok(result);
    }

}
