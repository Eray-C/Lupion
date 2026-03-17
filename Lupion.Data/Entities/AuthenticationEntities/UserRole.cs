namespace Empty_ERP_Template.Data.Entities.AuthenticationEntities;

public class UserRole : Entity<int>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }
}
