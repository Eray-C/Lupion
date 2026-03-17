using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

public class PersonnelAdvanceProfile : Profile
{
    public PersonnelAdvanceProfile()
    {
        CreateMap<PersonnelAdvance, PersonnelAdvanceDTO>()
            .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Name : null));
        CreateMap<PersonnelAdvanceRequest, PersonnelAdvance>()
            .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.RemainingAmount, opt => opt.Ignore())
            .ForMember(dest => dest.AdvanceClosedDate, opt => opt.Ignore());
        CreateMap<PersonnelAdvance, PersonnelAdvanceRequest>();
        CreateMap<PersonnelAdvance, PersonnelAdvanceRequest>();
    }
}
