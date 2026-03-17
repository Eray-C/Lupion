namespace Empty_ERP_Template.Business.DTOs.Customer;

public class CustomerAllDataDTO
{
    public CustomerDTO Customer { get; set; }
    public IEnumerable<CustomerContractDTO>? CustomerContracts { get; set; }
    public IEnumerable<CustomerPriceDTO>? Prices { get; set; }
}
