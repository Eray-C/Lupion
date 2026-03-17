namespace Empty_ERP_Template.Business.DTOs.Personnel;

public class PersonnelAdvanceDTO
{
    public int Id { get; set; }
    public int PersonnelId { get; set; }
    public DateTime AdvanceDate { get; set; }
    public DateTime StartDeductionDate { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public decimal GivenAmount { get; set; }
    public int DeductionMonths { get; set; }
    public decimal DeductionAmountPerMonth { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime? AdvanceClosedDate { get; set; }
}
