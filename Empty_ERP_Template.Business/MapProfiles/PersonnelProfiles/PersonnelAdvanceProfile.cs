using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

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
