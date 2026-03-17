namespace Empty_ERP_Template.Business.Requests.Personnel;

public class PersonnelPayrollBatchRequest
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Note { get; set; }
    public IEnumerable<PersonnelPayrollRequest> Payrolls { get; set; } = Array.Empty<PersonnelPayrollRequest>();
}
