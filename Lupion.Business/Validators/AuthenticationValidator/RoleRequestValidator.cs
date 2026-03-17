using Lupion.Business.Requests.Authentication;
using FluentValidation;

namespace Lupion.Business.Validators.AuthenticationValidator;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Rol adÄ± zorunludur.").MinimumLength(2).WithMessage("Rol adÄ± en az 2 karakter olmalÄ±dÄ±r.");
    }
}
