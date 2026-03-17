namespace Lupion.Business.DTOs.Dashboard;

public record DashboardSummaryDto(
    DashboardSummaryCardDto TotalShipments,
    DashboardSummaryCardDto TotalRevenue,
    DashboardSummaryCardDto TotalCustomers,
    DashboardSummaryCardDto TotalPersonnels
    );
