namespace Lupion.Business.DTOs.Personnel;

public class PersonnelPayrollDTO
{
    public int? Id { get; set; }
    public int PersonnelId { get; set; }
    public string? PersonnelName { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal? CalculatedNetSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? MealCardFee { get; set; }
    public decimal? TravelExpense { get; set; }
    public decimal? TotalPayableAmount { get; set; }
    public decimal TotalBonus { get; set; }
    public string? BonusDetails { get; set; }
    public decimal TotalDeduction { get; set; }
    public string? DeductionDetails { get; set; }
    public decimal AdvanceDeduction { get; set; }
    public string? AdvanceDetails { get; set; }
    public int? PeriodYear { get; set; }
    public int? PeriodMonth { get; set; }
    public string? Provider { get; set; }
    public string? Note { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
