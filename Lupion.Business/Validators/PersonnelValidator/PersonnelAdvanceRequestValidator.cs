using Lupion.Business.Requests.Personnel;
using FluentValidation;

namespace Lupion.Business.Validators.PersonnelValidator;

public class PersonnelAdvanceRequestValidator : AbstractValidator<PersonnelAdvanceRequest>
{
    public PersonnelAdvanceRequestValidator()
    {
        RuleFor(x => x.PersonnelId).GreaterThan(0).WithMessage("Personel seÃ§ilmelidir.");
        RuleFor(x => x.AdvanceDate).NotEmpty().WithMessage("Avans tarihi girilmelidir.");
        RuleFor(x => x.StartDeductionDate).NotEmpty().WithMessage("Kesinti baÅŸlangÄ±Ã§ tarihi girilmelidir.");
        RuleFor(x => x.GivenAmount).GreaterThanOrEqualTo(0).WithMessage("Verilen tutar negatif olamaz.");
        RuleFor(x => x.DeductionMonths).GreaterThan(0).WithMessage("Kesinti ayÄ± 0'dan bÃ¼yÃ¼k olmalÄ±dÄ±r.");
        RuleFor(x => x.DeductionAmountPerMonth).GreaterThanOrEqualTo(0).WithMessage("AylÄ±k kesinti tutarÄ± negatif olamaz.");
        RuleFor(x => x.Notes).MaximumLength(1000).WithMessage("Not 1000 karakterden uzun olamaz.");
    }
}
