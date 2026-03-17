using Empty_ERP_Template.Business.Requests.Personnel;
using FluentValidation;

namespace Empty_ERP_Template.Business.Validators.PersonnelValidator;

public class PersonnelAdvanceRequestValidator : AbstractValidator<PersonnelAdvanceRequest>
{
    public PersonnelAdvanceRequestValidator()
    {
        RuleFor(x => x.PersonnelId).GreaterThan(0).WithMessage("Personel seçilmelidir.");
        RuleFor(x => x.AdvanceDate).NotEmpty().WithMessage("Avans tarihi girilmelidir.");
        RuleFor(x => x.StartDeductionDate).NotEmpty().WithMessage("Kesinti başlangıç tarihi girilmelidir.");
        RuleFor(x => x.GivenAmount).GreaterThanOrEqualTo(0).WithMessage("Verilen tutar negatif olamaz.");
        RuleFor(x => x.DeductionMonths).GreaterThan(0).WithMessage("Kesinti ayı 0'dan büyük olmalıdır.");
        RuleFor(x => x.DeductionAmountPerMonth).GreaterThanOrEqualTo(0).WithMessage("Aylık kesinti tutarı negatif olamaz.");
        RuleFor(x => x.Notes).MaximumLength(1000).WithMessage("Not 1000 karakterden uzun olamaz.");
    }
}
