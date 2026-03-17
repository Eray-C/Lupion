using AutoMapper;
using Lupion.Business.DTOs.Authentication;
using Lupion.Business.DTOs.Shared;
using Lupion.Business.Requests.Authentication;
using Lupion.Business.Requests.Shared;
using Lupion.Data.Entities.AuthenticationEntities;
using Lupion.Data.Entities.SharedEntities;

namespace Lupion.Business.MapProfiles;

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
