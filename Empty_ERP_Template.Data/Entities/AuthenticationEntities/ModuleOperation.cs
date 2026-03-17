namespace Empty_ERP_Template.Data.Entities.AuthenticationEntities;

public class ModuleOperation
{
    public int Id { get; set; }
    public int? ModuleId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Module? Module { get; set; }
}
