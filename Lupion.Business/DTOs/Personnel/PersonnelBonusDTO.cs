namespace Lupion.Business.DTOs.Personnel;

public class PersonnelBonusDTO
{
    public int Id { get; set; }
    public int PersonnelId { get; set; }
    public int TypeId { get; set; }
    public string? TypeName { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string? MonthName { get; set; }
    public string? Notes { get; set; }
}
