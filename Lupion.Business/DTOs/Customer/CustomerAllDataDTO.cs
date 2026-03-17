namespace Lupion.Business.DTOs.Customer;

public class CustomerAllDataDTO
{
    public CustomerDTO Customer { get; set; }
    public IEnumerable<CustomerContractDTO>? CustomerContracts { get; set; }
    public IEnumerable<CustomerPriceDTO>? Prices { get; set; }
}
