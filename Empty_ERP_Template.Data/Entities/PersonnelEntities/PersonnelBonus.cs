using Empty_ERP_Template.Data.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

[Table("PersonnelBonuses")]
public class PersonnelBonus : Entity<int>
{
    public int PersonnelId { get; set; }
    public int TypeId { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Notes { get; set; }

    public virtual GeneralType? Type { get; set; }
    public virtual Currency? Currency { get; set; }
}
