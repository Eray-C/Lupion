using System.ComponentModel.DataAnnotations;

namespace Empty_ERP_Template.Data.Entities.SharedEntities;

public class Menu
{
    [Key]
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string? Text { get; set; }
    public string? Icon { get; set; }
    public string? URL { get; set; }
    public string? ModuleCode { get; set; }
    public string? Type { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public string? Title { get; set; }
}
