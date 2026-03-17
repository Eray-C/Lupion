namespace Empty_ERP_Template.Business.DTOs.Authentication;

public class RolePermissionGridItemDTO
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public int OperationId { get; set; }
    public string OperationName { get; set; } = string.Empty;
    public string OperationDisplayName { get; set; } = string.Empty;
    public bool Checked { get; set; }
}
