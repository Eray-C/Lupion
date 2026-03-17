namespace Lupion.Business.DTOs.Dashboard;

public record UpcomingShipmentMaterialTypeDto(
    string MaterialType,
    int ShipmentCount,
    decimal TotalFreightPrice);
