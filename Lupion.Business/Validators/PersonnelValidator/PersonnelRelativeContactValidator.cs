using Lupion.Business.Requests.Personnel;
using FluentValidation;

namespace Lupion.Business.Validators.PersonnelValidator;

public class PersonnelRelativeContactValidator : AbstractValidator<PersonnelRelativeContactRequest>
{
    public PersonnelRelativeContactValidator()
    {
        RuleFor(x => x.PersonnelId).GreaterThan(0).WithMessage("Personel seçilmelidir.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad boş olamaz.").MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad boş olamaz.").MaximumLength(100);

        RuleFor(x => x.ContactEmail)
            .EmailAddress().WithMessage("E-posta adresi geçerli formatta olmalıdır.")
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));

        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("Telefon en fazla 50 karakter olabilir.");

        RuleFor(x => x.IdentityNumber)
            .MaximumLength(50).WithMessage("Kimlik numarası en fazla 50 karakter olabilir.");

        RuleFor(x => x.Address)
            .MaximumLength(250).WithMessage("Adres en fazla 250 karakter olabilir.");
    }
}
