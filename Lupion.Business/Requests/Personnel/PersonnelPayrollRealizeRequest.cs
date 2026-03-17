namespace Empty_ERP_Template.Business.Requests.Personnel;

public class PersonnelPayrollRealizeRequest
{
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime RealizedDate { get; set; }
    public string? Notes { get; set; }
}
