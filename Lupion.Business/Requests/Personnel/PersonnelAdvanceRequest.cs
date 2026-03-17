namespace Empty_ERP_Template.Business.Requests.Personnel;

public class PersonnelAdvanceRequest
{
    public int? Id { get; set; }
    public int PersonnelId { get; set; }
    public DateTime AdvanceDate { get; set; }
    public DateTime StartDeductionDate { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal GivenAmount { get; set; }
    public int DeductionMonths { get; set; }
    public decimal DeductionAmountPerMonth { get; set; }
    public string? Notes { get; set; }
}
