namespace Empty_ERP_Template.Business.Requests.Personnel;

public class PersonnelAdvancePaymentRequest
{
    public int PersonnelId { get; set; }
    public int PersonnelAdvanceId { get; set; }
    public decimal Amount { get; set; }
    public int PeriodYear { get; set; }
    public int PeriodMonth { get; set; }
    public int? PersonnelPayrollId { get; set; }
    public string? Notes { get; set; }
}
