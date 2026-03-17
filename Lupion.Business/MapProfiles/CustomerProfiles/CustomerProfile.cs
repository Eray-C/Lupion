using AutoMapper;
using Lupion.Business.DTOs.Customer;
using Lupion.Business.Requests.Customer;
using Lupion.Data.Entities.CustomerEntities;

namespace Lupion.Business.MapProfiles.CustomerProfiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDTO>().ReverseMap();
        CreateMap<CustomerContract, CustomerContractDTO>().ReverseMap();
        CreateMap<CustomerContractRequest, CustomerContract>().ReverseMap();
        CreateMap<CustomerRequest, Customer>().ReverseMap();

        CreateMap<CustomerPriceRequest, CustomerPrice>().ReverseMap();
        CreateMap<CustomerPrice, CustomerPriceDTO>();

    }
}
