using Lupion.Data.Entities.SharedEntities;


namespace Lupion.Data.Entities.PersonnelEntities;

public class PersonnelLicense : Entity<int>
{
    public int PersonnelId { get; set; }
    public int LicenseTypeId { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Category { get; set; }
    public string? IssuedBy { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Notes { get; set; }
    public required virtual GeneralType LicenseType { get; set; }
}
