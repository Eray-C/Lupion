namespace Lupion.Data.Entities.CustomerEntities;

public class Customer() : Entity<int>
{
    public string? Name { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? BankName { get; set; }
    public string? BankNumber { get; set; }
    public string? BankAccountNo { get; set; }
}
