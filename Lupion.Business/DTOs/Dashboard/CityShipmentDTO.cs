namespace Empty_ERP_Template.Business.DTOs.Dashboard;
public record CityShipmentDTO
{
    public required string CityCode { get; set; }
    public required string CityName { get; set; }
    public int ShipmentCount { get; set; }
}