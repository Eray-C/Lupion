using Empty_ERP_Template.Business.Requests.Personnel;
using FluentValidation;

namespace Empty_ERP_Template.Business.Validators.PersonnelValidator;

public class PersonnelRelativeContactValidator : AbstractValidator<PersonnelRelativeContactRequest>
{
    public PersonnelRelativeContactValidator()
    {
        RuleFor(x => x.PersonnelId).GreaterThan(0).WithMessage("Personel seçilmelidir.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad boþ olamaz.").MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad boþ olamaz.").MaximumLength(100);

        RuleFor(x => x.ContactEmail)
            .EmailAddress().WithMessage("E-posta adresi geçerli formatta olmalýdýr.")
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));

        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("Telefon en fazla 50 karakter olabilir.");

        RuleFor(x => x.IdentityNumber)
            .MaximumLength(50).WithMessage("Kimlik numarasý en fazla 50 karakter olabilir.");

        RuleFor(x => x.Address)
            .MaximumLength(250).WithMessage("Adres en fazla 250 karakter olabilir.");
    }
}
