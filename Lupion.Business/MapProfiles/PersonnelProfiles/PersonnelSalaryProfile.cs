using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles
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
