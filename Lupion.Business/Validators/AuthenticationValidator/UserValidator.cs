using Empty_ERP_Template.Business.Requests.Authentication;
using Empty_ERP_Template.Business.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Empty_ERP_Template.Business.Validators.AuthenticationValidator;

public class UserValidator : AbstractValidator<RegisterRequest>
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserValidator(UserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;

        RuleFor(x => x.Email)
            .MustAsync(BeUniqueEmail)
            .WithMessage("Bu email zaten kayýtlý")
            .When(_ => _httpContextAccessor.HttpContext?.Request.Method == "POST");


        RuleFor(x => x.Username)
            .MustAsync(BeUniqueUsername)
            .WithMessage("Bu kullanýcý adý zaten kayýtlý")
            .When(_ => _httpContextAccessor.HttpContext?.Request.Method == "POST");


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Þifre zorunludur")
            .When(_ => _httpContextAccessor.HttpContext?.Request.Method == "POST");
    }

    private async Task<bool> BeUniqueEmail(RegisterRequest request, string email, CancellationToken cancellationToken)
    {
        return !await _userService.IsAnyUserWithThisEmail(email, request.Id ?? default);
    }

    private async Task<bool> BeUniqueUsername(RegisterRequest request, string username, CancellationToken cancellationToken)
    {
        return !await _userService.IsAnyUserWithThisUsername(username, request.Id ?? default);
    }
}