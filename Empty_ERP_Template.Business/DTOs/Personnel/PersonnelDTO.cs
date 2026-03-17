namespace Empty_ERP_Template.Business.DTOs.Personnel
{
    public class PersonnelDTO
    {

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
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
        public string? GenderName { get; set; }
        public string? MaritalStatusName { get; set; }
        public string? DepartmentName { get; set; }
        public string? PersonnelTypeName { get; set; }
        public string? StatusName { get; set; }
    }
}
