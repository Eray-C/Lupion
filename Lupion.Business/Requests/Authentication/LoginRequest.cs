namespace Lupion.Business.Requests.Authentication;

public class LoginRequest(string emailOrUserName, string password)
{
    public string EmailOrUserName { get; set; } = emailOrUserName;
    public string Password { get; set; } = password;
}
