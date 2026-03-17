using Empty_ERP_Template.Business.Interfaces;

namespace Empty_ERP_Template.Business.Requests.Authentication;
public class RegisterRequest : IHasId
{
    public int? Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; }
    public required List<int> RoleIds { get; set; }
    public required bool IsActive { get; set; }
}
