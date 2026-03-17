using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

public class PersonnelRelativeContactProfile : Profile
{
    public PersonnelRelativeContactProfile()
    {
        CreateMap<PersonnelRelativeContact, PersonnelRelativeContactDTO>()
            .ForMember(dest => dest.RelationshipTypeName,
                       opt => opt.MapFrom(src => src.RelationshipType != null ? src.RelationshipType.Name : null))
            .ForMember(dest => dest.GenderTypeName,
                       opt => opt.MapFrom(src => src.GenderType != null ? src.GenderType.Name : null))
            .ForMember(dest => dest.FullName,
                       opt => opt.MapFrom(src => (src.FirstName + " " + src.LastName).Trim()))
            .ReverseMap();

        CreateMap<PersonnelRelativeContactRequest, PersonnelRelativeContact>().ReverseMap();
    }
}
