using AutoMapper;
using Lupion.Business.Common;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

public class PersonnelBonusProfile : Profile
{
    public PersonnelBonusProfile()
    {
        CreateMap<PersonnelBonus, PersonnelBonusDTO>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : null))
            .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Name : null))
            .ForMember(dest => dest.MonthName, opt => opt.MapFrom(src => TurkishMonthHelper.GetName(src.Month)));
        CreateMap<PersonnelBonusRequest, PersonnelBonus>().ReverseMap();
        CreateMap<PersonnelBonus, PersonnelBonusRequest>();
    }
}
