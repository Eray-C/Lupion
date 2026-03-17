using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Business.Requests.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

public class PersonnelContactProfile : Profile
{
    public PersonnelContactProfile()
    {
        CreateMap<PersonnelContact, PersonnelContactDTO>().ReverseMap();
        CreateMap<PersonnelContact, PersonnelContactRequest>().ReverseMap();
    }
}
