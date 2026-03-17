namespace Empty_ERP_Template.Business.Requests.Authentication;

public class ForgotPasswordRequest(string email)
{
    public string Email { get; set; } = email;
}
