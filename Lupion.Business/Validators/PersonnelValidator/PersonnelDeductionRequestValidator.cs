using Lupion.Business.Requests.Personnel;
using FluentValidation;

namespace Lupion.Business.Validators.PersonnelValidator;

public class PersonnelDeductionRequestValidator : AbstractValidator<PersonnelDeductionRequest>
{
    public PersonnelDeductionRequestValidator()
    {
        RuleFor(x => x.PersonnelId).GreaterThan(0).WithMessage("Personel seçilmelidir.");
        RuleFor(x => x.TypeId).GreaterThan(0).WithMessage("Kesinti tipi seçilmelidir.");
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Tutar negatif olamaz.");
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100).WithMessage("Geçerli bir yıl giriniz.");
        RuleFor(x => x.Month).InclusiveBetween(1, 12).WithMessage("Ay 1-12 arasında olmalıdır.");
        RuleFor(x => x.Notes).MaximumLength(1000).WithMessage("Not 1000 karakterden uzun olamaz.");
    }
}
