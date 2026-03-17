using Lupion.Business.Requests.Authentication;
using FluentValidation;

namespace Lupion.Business.Validators.AuthenticationValidator;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Rol adı zorunludur.").MinimumLength(2).WithMessage("Rol adı en az 2 karakter olmalıdır.");
    }
}
