namespace Empty_ERP_Template.Business.DTOs.Personnel;

public class PersonnelAdvancePaymentDTO
{
    public int Id { get; set; }
    public int PersonnelId { get; set; }
    public int PersonnelAdvanceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PeriodYear { get; set; }
    public int PeriodMonth { get; set; }
    public int? PersonnelPayrollId { get; set; }
    public string? Notes { get; set; }
    public string? SourceInfo { get; set; }
}
