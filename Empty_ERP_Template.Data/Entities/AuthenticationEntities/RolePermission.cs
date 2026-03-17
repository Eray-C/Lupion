using System.ComponentModel.DataAnnotations;

namespace Empty_ERP_Template.Data.Entities.AuthenticationEntities;

public class RolePermission
{
    [Key]
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int ModuleOperationId { get; set; }
}
