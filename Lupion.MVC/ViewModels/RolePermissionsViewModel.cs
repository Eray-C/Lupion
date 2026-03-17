using Lupion.Business.DTOs.Authentication;

namespace Lupion.MVC.ViewModels;

public class RolePermissionsViewModel
{
    public IEnumerable<RoleDTO> Roles { get; set; }
    public IEnumerable<ModuleDTO> Modules { get; set; }
}
