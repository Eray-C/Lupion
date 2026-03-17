using Empty_ERP_Template.Business.Services;
using Empty_ERP_Template.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.MVC.Controllers;

[Route("settings")]
public class SettingController(SettingsService settingsService) : BaseController
{
    [HttpGet("users/add-edit")]
    public async Task<IActionResult> Users()
    {
        var roles = await settingsService.GetRolesAsync();
        return View("User/frmUserAddEdit", roles);
    }

    [HttpGet("roles/add-edit")]
    public IActionResult RoleAddEdit()
    {
        return View("Role/frmRoleAddEdit");
    }

    [HttpGet("role-permissions-view")]
    public async Task<IActionResult> RolePermissionsView()
    {
        var roles = await settingsService.GetRolesAsync();
        var model = new RolePermissionsViewModel { Roles = roles, Modules = [] };
        return View("RolePermission/frmRolePermissions", model);
    }
}
