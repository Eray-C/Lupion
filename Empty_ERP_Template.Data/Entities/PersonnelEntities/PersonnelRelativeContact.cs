using Empty_ERP_Template.Data.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;


namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

[Table("PersonnelRelativeContacts")]
public class PersonnelRelativeContact : Entity<int>
{
    public int PersonnelId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? RelationshipTypeId { get; set; }
    public int? GenderTypeId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? IdentityNumber { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? Address { get; set; }
    public bool IsEmergencyContact { get; set; }
    public string? Notes { get; set; }

    public GeneralType RelationshipType { get; set; }
    public GeneralType GenderType { get; set; }
}