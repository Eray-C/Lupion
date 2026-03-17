namespace Empty_ERP_Template.Business.Requests.Authentication;

public class SaveRolePermissionsRequest
{
    public int RoleId { get; set; }
    public List<RolePermissionItemRequest>? Permissions { get; set; }
    public List<RolePermissionGridItemRequest>? GridItems { get; set; }
}

public class RolePermissionItemRequest
{
    public int ModuleId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanApprove { get; set; }
    public bool CanExport { get; set; }
}

public class RolePermissionGridItemRequest
{
    public int ModuleId { get; set; }
    public int OperationId { get; set; }
    public bool Checked { get; set; }
}
