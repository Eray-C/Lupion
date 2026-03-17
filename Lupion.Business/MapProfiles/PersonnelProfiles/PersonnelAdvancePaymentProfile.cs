using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

public class PersonnelAdvancePaymentProfile : Profile
{
    public PersonnelAdvancePaymentProfile()
    {
        CreateMap<PersonnelAdvancePayment, PersonnelAdvancePaymentDTO>()
            .ForMember(dest => dest.SourceInfo, opt => opt.MapFrom(src => src.PersonnelPayrollId != null ? "Bordrodan ödendi" : "Manuel"));
        CreateMap<PersonnelAdvancePaymentRequest, PersonnelAdvancePayment>();
    }
}
