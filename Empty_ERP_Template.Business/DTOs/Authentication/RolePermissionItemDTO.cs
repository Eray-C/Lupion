namespace Empty_ERP_Template.Business.DTOs.Authentication;

public class RolePermissionItemDTO
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; }
    public string OperationName { get; internal set; }
    public string? Description { get; set; }
    public int OperationId { get; internal set; }
    public bool Checked { get; internal set; }
}
