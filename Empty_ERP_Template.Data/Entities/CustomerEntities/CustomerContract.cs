namespace Empty_ERP_Template.Data.Entities.CustomerEntities;

public class CustomerContract() : Entity<int>
{
    public int CustomerId { get; set; }
    public string? ContractNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? FreightTerms { get; set; }
    public string? PriceList { get; set; }
}
