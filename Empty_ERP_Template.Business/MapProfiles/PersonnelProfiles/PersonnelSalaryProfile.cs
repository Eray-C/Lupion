using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles
{
    public class PersonnelSalaryProfile : Profile
    {
        public PersonnelSalaryProfile()
        {
            CreateMap<PersonnelSalary, PersonnelSalaryDTO>()
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency.Name))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType.Name));

            CreateMap<PersonnelSalaryRequest, PersonnelSalary>().ReverseMap();

            CreateMap<PersonnelSalary, PersonnelSalaryRequest>();
        }
    }
}
