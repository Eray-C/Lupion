namespace Lupion.Business.DTOs.Dashboard;

public record DashboardSummaryCardDto(
    decimal Value,
    decimal WeeklyDifference,
    bool IsIncrease,
    string ValueFormatted,
    string DifferenceText,
    string Format);
