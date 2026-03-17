using Lupion.Business.Interfaces;

namespace Lupion.Business.Requests.Personnel;

public class PersonnelRequest : IHasId
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? GenderTypeId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PlaceOfBirth { get; set; }
    public string? Nationality { get; set; }
    public int? MaritalStatusTypeId { get; set; }
    public string? IdentityNumber { get; set; }
    public int? PersonnelTypeId { get; set; }
    public int? DepartmentTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
    private byte[]? _photo;
    public byte[]? Photo
    {
        get => _photo;
        set
        {
            if (value == null || value.Length == 0)
            {
                _photo = ExistingPhoto;
            }
            else
            {
                _photo = value;
            }
        }
    }
    public byte[]? ExistingPhoto { get; set; }

    public int? StatusTypeId { get; set; }
}
