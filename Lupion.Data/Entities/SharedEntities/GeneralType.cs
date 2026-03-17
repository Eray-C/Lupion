using System.ComponentModel.DataAnnotations.Schema;

namespace Empty_ERP_Template.Data.Entities.SharedEntities;

[Table("Types")]
public class GeneralType : Entity<int>
{
    public int? ParentId { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public int? Order { get; set; }
}

