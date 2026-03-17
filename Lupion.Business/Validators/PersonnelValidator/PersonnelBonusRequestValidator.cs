using Empty_ERP_Template.Business.Requests.Personnel;
using FluentValidation;

namespace Empty_ERP_Template.Business.Validators.PersonnelValidator;

public class PersonnelBonusRequestValidator : AbstractValidator<PersonnelBonusRequest>
{
    public PersonnelBonusRequestValidator()
    {
        RuleFor(x => x.PersonnelId).GreaterThan(0).WithMessage("Personel seçilmelidir.");
        RuleFor(x => x.TypeId).GreaterThan(0).WithMessage("Bonus tipi seçilmelidir.");
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Tutar negatif olamaz.");
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100).WithMessage("Geçerli bir yıl giriniz.");
        RuleFor(x => x.Month).InclusiveBetween(1, 12).WithMessage("Ay 1-12 arasında olmalıdır.");
        RuleFor(x => x.Notes).MaximumLength(1000).WithMessage("Not 1000 karakterden uzun olamaz.");
    }
}
