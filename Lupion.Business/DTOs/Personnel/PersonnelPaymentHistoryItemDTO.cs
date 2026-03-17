namespace Lupion.Business.DTOs.Personnel;

public class PersonnelPaymentHistoryItemDTO
{
    public int Id { get; set; }
    public int PeriodYear { get; set; }
    public int PeriodMonth { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public DateTime RealizedDate { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? TravelExpense { get; set; }
    public decimal BonusTotal { get; set; }
    public decimal DeductionTotal { get; set; }
    public decimal AdvanceDeductionTotal { get; set; }
    public decimal? TotalPayable { get; set; }
    public string? Notes { get; set; }
}
