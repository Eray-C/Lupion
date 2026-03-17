namespace Lupion.Data.Entities.CustomerEntities;

public class CustomerPrice : Entity<int>
{
    public int CustomerId { get; set; }
    public required string Name { get; set; }
    public int? CurrencyId { get; set; }
    public decimal? Price { get; set; }
    public string? Note { get; set; }
    public string? DepartureRegion { get; set; }
    public string? ArrivalRegion { get; set; }
    public string? DepartureCompany { get; set; }
    public string? ArrivalCompany { get; set; }
    public int? DepartureCityId { get; set; }
    public int? DepartureTownId { get; set; }
    public int? ArrivalCityId { get; set; }
    public int? ArrivalTownId { get; set; }
}
