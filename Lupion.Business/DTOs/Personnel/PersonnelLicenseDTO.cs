namespace Lupion.Business.DTOs.Personnel;
public class PersonnelLicenseDTO
{
    public int Id { get; set; }
    public int PersonnelId { get; set; }
    public int LicenseTypeId { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Category { get; set; }
    public string? IssuedBy { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Notes { get; set; }

    public string? LicenseTypeName { get; set; }
}
