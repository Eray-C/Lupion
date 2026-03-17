namespace Empty_ERP_Template.Business.Requests.Personnel;

public class PersonnelPayrollRequest
{
    public int? Id { get; set; }
    public int PersonnelId { get; set; }
    public string? PersonnelName { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal? CalculatedNetSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? MealCardFee { get; set; }
    public decimal? TravelExpense { get; set; }
    public decimal? TotalPayableAmount { get; set; }
    public decimal? TotalBonus { get; set; }
    public string? BonusDetails { get; set; }
    public decimal? TotalDeduction { get; set; }
    public string? DeductionDetails { get; set; }
    public decimal? AdvanceDeduction { get; set; }
    public string? AdvanceDetails { get; set; }
    public string? Provider { get; set; }
}
