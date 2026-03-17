using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

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
