using System.ComponentModel.DataAnnotations.Schema;

namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

[Table("PersonnelAdvancePayments")]
public class PersonnelAdvancePayment : Entity<int>
{
    public int PersonnelId { get; set; }
    public int PersonnelAdvanceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PeriodYear { get; set; }
    public int PeriodMonth { get; set; }
    public int? PersonnelPayrollId { get; set; }
    public string? Notes { get; set; }

    public virtual PersonnelAdvance? PersonnelAdvance { get; set; }
    public virtual PersonnelPayroll? PersonnelPayroll { get; set; }
}
