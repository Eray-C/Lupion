namespace Empty_ERP_Template.Data.Entities.AuthenticationEntities;

public class PasswordResetToken(int userId, string token, DateTime expiresAt)
{
    public int Id { get; set; }
    public int UserId { get; set; } = userId;
    public string Token { get; set; } = token;
    public DateTime ExpiresAt { get; set; } = expiresAt;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int FailedAttempts { get; set; }
    public bool Used { get; set; }
    public User User { get; set; }
}
