using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Customer;
using Empty_ERP_Template.Business.Requests.Customer;
using Empty_ERP_Template.Data.Entities.CustomerEntities;

namespace Empty_ERP_Template.Business.MapProfiles.CustomerProfiles;

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
