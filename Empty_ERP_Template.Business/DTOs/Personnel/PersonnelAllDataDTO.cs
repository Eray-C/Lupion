namespace Empty_ERP_Template.Business.DTOs.Personnel;

public class PersonnelAllDataDTO
{
    public IEnumerable<PersonnelRelativeContactDTO> Relatives { get; set; }
    public IEnumerable<PersonnelContactDTO> Contacts { get; set; }
    public IEnumerable<PersonnelSalaryDTO> Salaries { get; set; }
    public IEnumerable<PersonnelBonusDTO> Bonuses { get; set; }
    public IEnumerable<PersonnelDeductionDTO> Deductions { get; set; }
    public IEnumerable<PersonnelAdvanceDTO> Advances { get; set; }
    public IEnumerable<PersonnelPaymentHistoryItemDTO> PaymentHistory { get; set; }
    public IEnumerable<PersonnelLicenseDTO> Licences { get; set; }
    public PersonnelDTO Personnel { get; set; }
}
