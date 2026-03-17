using Lupion.Business.Requests.Authentication;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[ApiController]
[Route("api/settings")]
public class SettingController(UserService userService, SettingsService settingsService) : BaseController
{
    [HttpGet("users")]
    public async Task<IActionResult> QueryUsersAsync()
    {
        var users = await userService.QueryUsersAsync();
        return Ok(users);
    }

    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        await userService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRolesAsync()
    {
        var roles = await settingsService.GetRolesAsync();
        return Ok(roles);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleRequest request)
    {
        await settingsService.CreateRoleAsync(request);
        return Ok("Rol eklendi");
    }

    [HttpPut("roles/{id:int}")]
    public async Task<IActionResult> UpdateRoleAsync(int id, [FromBody] RoleRequest request)
    {
        await settingsService.UpdateRoleAsync(id, request);
        return Ok("Rol güncellendi");
    }

    [HttpDelete("roles/{id:int}")]
    public async Task<IActionResult> DeleteRoleAsync(int id)
    {
        await settingsService.DeleteRoleAsync(id);
        return Ok();
    }


 
    [HttpGet("role-permissions/{roleId:int}")]
    public async Task<IActionResult> GetRolePermissionsAsync(int roleId)
    {
        var list = await settingsService.GetRolePermissionsAsync(roleId);
        return Ok(list);
    }

    [HttpPut("role-permissions")]
    public async Task<IActionResult> SaveRolePermissionsAsync([FromBody] SaveRolePermissionsRequest request)
    {
        await settingsService.SaveRolePermissionsAsync(request);
        return Ok("İzinler kaydedildi");
    }
}
