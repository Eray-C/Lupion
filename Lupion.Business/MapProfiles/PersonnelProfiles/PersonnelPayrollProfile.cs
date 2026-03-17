using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

public class PersonnelPayrollProfile : Profile
{
    public PersonnelPayrollProfile()
    {
        CreateMap<PaidPayroll, PayrollPeriodListDTO>()
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.TotalPaidAmount.HasValue || !string.IsNullOrEmpty(src.PaymentNote)))
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy ?? src.CreatedBy))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate ?? src.CreatedDate))
            .ForMember(dest => dest.UpdatedByName, opt => opt.MapFrom<PayrollUpdatedByNameResolver>());

        CreateMap<PersonnelPayroll, PersonnelPayrollDTO>();

        CreateMap<PersonnelPayrollRequest, PersonnelPayroll>()
            .ForMember(dest => dest.PeriodMonth, opt => opt.Ignore())
            .ForMember(dest => dest.PeriodYear, opt => opt.Ignore())
            .ForMember(dest => dest.Personnel, opt => opt.Ignore())
            .ForMember(dest => dest.Note, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
    }
}
