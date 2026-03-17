namespace Empty_ERP_Template.Business.DTOs.Customer;

public class CustomerContractDTO()
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? ContractNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? FreightTerms { get; set; }
    public string? PriceList { get; set; }
}
