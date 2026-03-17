using Empty_ERP_Template.Data.Entities;

namespace Empty_ERP_Template.Business.DTOs.Authentication;
public class UserDTO : Entity<int>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string CompanyCode { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public IEnumerable<int> RoleIds { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}
