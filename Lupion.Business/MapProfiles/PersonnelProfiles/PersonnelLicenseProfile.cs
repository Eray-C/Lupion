using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

public class PersonnelLicenseProfile : Profile
{
    public PersonnelLicenseProfile()
    {
        CreateMap<PersonnelLicense, PersonnelLicenseDTO>()
            .ForMember(dest => dest.LicenseTypeName, opt => opt.MapFrom(src => src.LicenseType.Name));

        CreateMap<PersonnelLicenseRequest, PersonnelLicense>().ReverseMap();


        CreateMap<PersonnelLicense, PersonnelLicenseRequest>();
    }
}
