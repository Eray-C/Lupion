using AutoMapper;
using Empty_ERP_Template.Business.Common;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

public class PersonnelDeductionProfile : Profile
{
    public PersonnelDeductionProfile()
    {
        CreateMap<PersonnelDeduction, PersonnelDeductionDTO>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : null))
            .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Name : null))
            .ForMember(dest => dest.MonthName, opt => opt.MapFrom(src => TurkishMonthHelper.GetName(src.Month)));
        CreateMap<PersonnelDeductionRequest, PersonnelDeduction>().ReverseMap();
        CreateMap<PersonnelDeduction, PersonnelDeductionRequest>();
    }
}
