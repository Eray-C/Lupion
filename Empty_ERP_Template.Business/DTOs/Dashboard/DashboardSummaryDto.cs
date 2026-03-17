namespace Empty_ERP_Template.Business.DTOs.Dashboard;

public record DashboardSummaryDto(
    DashboardSummaryCardDto TotalShipments,
    DashboardSummaryCardDto TotalRevenue,
    DashboardSummaryCardDto TotalCustomers,
    DashboardSummaryCardDto TotalPersonnels
    );
