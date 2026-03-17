namespace Empty_ERP_Template.Business.DTOs.Dashboard;

public record UpcomingShipmentMaterialTypeDto(
    string MaterialType,
    int ShipmentCount,
    decimal TotalFreightPrice);
