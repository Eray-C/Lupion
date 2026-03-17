namespace Empty_ERP_Template.Data.Entities.AuthenticationEntities;

public class Role : Entity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
}
