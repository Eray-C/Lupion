using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

public class PersonnelProfile : Profile
{
    public PersonnelProfile()
    {
        CreateMap<Personnel, PersonnelDTO>()
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.GenderType.Name))
            .ForMember(dest => dest.MaritalStatusName, opt => opt.MapFrom(src => src.MaritalStatusType.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.DepartmentType.Name))
            .ForMember(dest => dest.PersonnelTypeName, opt => opt.MapFrom(src => src.PersonnelType.Name))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.StatusType.Name))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

        CreateMap<PersonnelRequest, Personnel>().ReverseMap();

    }
}
