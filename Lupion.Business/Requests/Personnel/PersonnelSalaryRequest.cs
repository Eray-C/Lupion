namespace Lupion.Business.Requests.Personnel;
public class PersonnelSalaryRequest
{
    public int? Id { get; set; }
    public int PersonnelId { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public decimal BaseSalary { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? MealCardFee { get; set; }
    public int? PaymentTypeId { get; set; }
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? Notes { get; set; }
}