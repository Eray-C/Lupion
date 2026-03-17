namespace Lupion.Business.DTOs.Dashboard;

public record UpcomingShipmentsByMaterialDto(
    string MonthName,
    IReadOnlyCollection<UpcomingShipmentMaterialTypeDto> Items);
