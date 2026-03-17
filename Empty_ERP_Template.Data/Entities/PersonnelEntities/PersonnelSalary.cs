using Empty_ERP_Template.Data.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

[Table("PersonnelSalaries")]
public class PersonnelSalary : Entity<int>
{
    public int PersonnelId { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public decimal BaseSalary { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? MealCardFee { get; set; }
    public int? PaymentTypeId { get; set; }
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? Notes { get; set; }

    public virtual Currency? Currency { get; set; }
    public virtual GeneralType? PaymentType { get; set; }
}
