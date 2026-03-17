using Empty_ERP_Template.Business.Requests.Personnel;
using FluentValidation;

namespace Empty_ERP_Template.Business.Validators.PersonnelValidator;

public class PersonnelRequestValidator : AbstractValidator<PersonnelRequest>
{
    public PersonnelRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boþ olamaz")
            .MaximumLength(100).WithMessage("Ad 100 karakterden uzun olamaz");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boþ olamaz")
            .MaximumLength(100).WithMessage("Soyad 100 karakterden uzun olamaz");

        RuleFor(x => x.IdentityNumber)
            .MaximumLength(50).WithMessage("Kimlik numarasý 50 karakterden uzun olamaz");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today).When(x => x.DateOfBirth.HasValue)
            .WithMessage("Doðum tarihi bugünden büyük olamaz");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Bitiþ tarihi baþlangýç tarihinden büyük olmalýdýr");
        RuleFor(x => x.StatusTypeId)
            .NotEmpty().WithMessage("Personnelin durumu boþ olamaz");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notlar 1000 karakterden uzun olamaz");
    }
}