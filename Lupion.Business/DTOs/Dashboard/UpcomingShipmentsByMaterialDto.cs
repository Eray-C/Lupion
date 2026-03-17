namespace Empty_ERP_Template.Business.DTOs.Dashboard;

public record UpcomingShipmentsByMaterialDto(
    string MonthName,
    IReadOnlyCollection<UpcomingShipmentMaterialTypeDto> Items);
