using System.ComponentModel.DataAnnotations.Schema;

namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

[Table("PersonnelContacts")]
public class PersonnelContact : Entity<int>
{
    public int PersonnelId { get; set; }
    public string? PersonalEmail { get; set; }
    public string? WorkEmail { get; set; }
    public string? MobilePhone { get; set; }
    public string? HomePhone { get; set; }
    public string? WorkPhone { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? CityId { get; set; }
    public string? StateId { get; set; }
    public string? CountryId { get; set; }
    public string? PostalCode { get; set; }
    public string? Notes { get; set; }

}
