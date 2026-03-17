using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

public class PersonnelAdvancePaymentProfile : Profile
{
    public PersonnelAdvancePaymentProfile()
    {
        CreateMap<PersonnelAdvancePayment, PersonnelAdvancePaymentDTO>()
            .ForMember(dest => dest.SourceInfo, opt => opt.MapFrom(src => src.PersonnelPayrollId != null ? "Bordrodan ödendi" : "Manuel"));
        CreateMap<PersonnelAdvancePaymentRequest, PersonnelAdvancePayment>();
    }
}
