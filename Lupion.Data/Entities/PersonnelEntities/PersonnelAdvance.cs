using Lupion.Data.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lupion.Data.Entities.PersonnelEntities;

[Table("PersonnelAdvances")]
public class PersonnelAdvance : Entity<int>
{
    public int PersonnelId { get; set; }
    public DateTime AdvanceDate { get; set; }
    public DateTime StartDeductionDate { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal GivenAmount { get; set; }
    public int DeductionMonths { get; set; }
    public decimal DeductionAmountPerMonth { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime? AdvanceClosedDate { get; set; }

    public virtual Currency? Currency { get; set; }
}
