using Empty_ERP_Template.Data.Entities;

namespace Empty_ERP_Template.Business.DTOs.Authentication;

public class RoleDTO : Entity<int>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
