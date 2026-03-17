using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

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
