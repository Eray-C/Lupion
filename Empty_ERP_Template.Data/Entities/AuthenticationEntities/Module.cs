namespace Empty_ERP_Template.Data.Entities.AuthenticationEntities;

public class Module
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<ModuleOperation> ModuleOperations { get; set; } = [];
}
