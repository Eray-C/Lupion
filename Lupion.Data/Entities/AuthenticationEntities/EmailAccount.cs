namespace Lupion.Data.Entities.AuthenticationEntities;
public class EmailAccount : Entity<int>
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string From { get; set; }
    public required bool EnableSsl { get; set; }
    public required bool IsActive { get; set; }
}
