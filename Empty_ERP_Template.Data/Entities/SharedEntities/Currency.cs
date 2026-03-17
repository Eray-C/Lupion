namespace Empty_ERP_Template.Data.Entities.SharedEntities;

public class Currency : Entity<int>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Symbol { get; set; }
    public bool IsActive { get; set; }
}