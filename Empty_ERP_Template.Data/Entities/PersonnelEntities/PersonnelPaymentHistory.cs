using System.ComponentModel.DataAnnotations.Schema;

namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

[Table("PersonnelPaymentHistory")]
public class PersonnelPaymentHistory : Entity<int>
{
    public int PersonnelId { get; set; }
    public int PersonnelPayrollId { get; set; }
    public int PeriodYear { get; set; }
    public int PeriodMonth { get; set; }
    public DateTime RealizedDate { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? TravelExpense { get; set; }
    public decimal BonusTotal { get; set; }
    public decimal DeductionTotal { get; set; }
    public decimal AdvanceDeductionTotal { get; set; }
    public decimal? TotalPaid { get; set; }
    public string? Notes { get; set; }

    public virtual PersonnelPayroll? PersonnelPayroll { get; set; }
}
