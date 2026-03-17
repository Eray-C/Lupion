namespace Lupion.Business.DTOs.Dashboard;
public class MonthlyShipmentsDTO
{
    public string PeriodDescription { get; set; }
    public List<MonthlyGroup> Months { get; set; }
}

public class MonthlyGroup
{
    public string MonthName { get; set; }
    public List<MonthlyItem> Items { get; set; }
}

public class MonthlyItem
{
    public string MaterialType { get; set; }
    public int ShipmentCount { get; set; }
    public decimal TotalFreightPrice { get; set; }
}

