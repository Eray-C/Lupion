namespace Lupion.Business.DTOs.Dashboard;

public record RecentShipmentDto(
    int Id,
    string? ShipmentNumber,
    DateTime? ShipmentDate,
    string? CustomerName,
    string? VehiclePlate,
    string? StatusName,
    decimal? FreightPrice,
    string? MaterialTypeName);
