using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Authentication;
using Empty_ERP_Template.Business.DTOs.Shared;
using Empty_ERP_Template.Business.Requests.Authentication;
using Empty_ERP_Template.Business.Requests.Shared;
using Empty_ERP_Template.Data.Entities.AuthenticationEntities;
using Empty_ERP_Template.Data.Entities.SharedEntities;

namespace Empty_ERP_Template.Business.MapProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GeneralType, GeneralTypeDTO>().ReverseMap();
        CreateMap<GeneralTypeRequest, GeneralType>().ReverseMap();
        CreateMap<RoleDTO, Role>().ReverseMap();
        CreateMap<RoleRequest, Role>();
        CreateMap<UserDTO, User>().ReverseMap();
    }
}
