namespace Lupion.Business.Requests.Personnel;

public class PersonnelDeductionRequest
{
    public int? Id { get; set; }
    public int PersonnelId { get; set; }
    public int TypeId { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Notes { get; set; }
}
