namespace Lupion.Data.Entities.AuthenticationEntities;

public class User : Entity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string CompanyCode { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public Company Company { get; set; }
}
