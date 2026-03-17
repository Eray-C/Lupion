namespace Empty_ERP_Template.Business.DTOs.Personnel;

public class PayrollPeriodListDTO
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Note { get; set; }
    public bool IsApproved { get; set; }
    public bool IsPaid { get; set; }
    public int? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public decimal? TotalPaidAmount { get; set; }
    public string? PaymentNote { get; set; }
}
