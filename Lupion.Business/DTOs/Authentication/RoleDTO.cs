using Lupion.Data.Entities;

namespace Lupion.Business.DTOs.Authentication;

public class RoleDTO : Entity<int>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
