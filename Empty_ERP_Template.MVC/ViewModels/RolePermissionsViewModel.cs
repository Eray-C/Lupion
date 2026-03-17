using Empty_ERP_Template.Business.DTOs.Authentication;

namespace Empty_ERP_Template.MVC.ViewModels;

public class RolePermissionsViewModel
{
    public IEnumerable<RoleDTO> Roles { get; set; }
    public IEnumerable<ModuleDTO> Modules { get; set; }
}
