using System.ComponentModel.DataAnnotations.Schema;

namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

[Table("PaidPayrolls")]
public class PaidPayroll : Entity<int>
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Note { get; set; }
    public string? PaymentNote { get; set; }
    public decimal? TotalPaidAmount { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
