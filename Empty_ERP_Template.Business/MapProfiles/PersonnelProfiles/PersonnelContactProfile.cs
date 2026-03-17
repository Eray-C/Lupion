using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Business.Requests.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

public class PersonnelContactProfile : Profile
{
    public PersonnelContactProfile()
    {
        CreateMap<PersonnelContact, PersonnelContactDTO>().ReverseMap();
        CreateMap<PersonnelContact, PersonnelContactRequest>().ReverseMap();
    }
}
