using Empty_ERP_Template.Data.Entities;

namespace Empty_ERP_Template.Business.DTOs.Shared;

public class GeneralTypeDTO : Entity<int>
{
    public string? Category { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string? Color { get; set; }
    public int? Order { get; set; }
    public int Count { get; set; }
}
