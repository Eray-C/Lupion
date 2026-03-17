namespace Empty_ERP_Template.Business.DTOs.Personnel;
public class PersonnelRelativeContactDTO
{
    public int Id { get; set; }
    public int PersonnelId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName { get; set; } = null!;

    public int? RelationshipTypeId { get; set; }
    public int? GenderTypeId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? IdentityNumber { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? Address { get; set; }
    public bool IsEmergencyContact { get; set; }
    public string? Notes { get; set; }
    public string? RelationshipTypeName { get; set; }
    public string? GenderTypeName { get; set; }
}