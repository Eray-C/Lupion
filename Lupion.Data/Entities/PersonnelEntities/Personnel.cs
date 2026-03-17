using Empty_ERP_Template.Data.Entities.SharedEntities;

namespace Empty_ERP_Template.Data.Entities.PersonnelEntities;

public class Personnel : Entity<int>
{
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
    public byte[]? Photo { get; set; }
    public int? StatusTypeId { get; set; }
    public virtual GeneralType? StatusType { get; set; }
    public virtual GeneralType? DepartmentType { get; set; }
    public virtual GeneralType? PersonnelType { get; set; }
    public virtual GeneralType? GenderType { get; set; }
    public virtual GeneralType? MaritalStatusType { get; set; }

}
