using Empty_ERP_Template.Business.Requests.Personnel;
using FluentValidation;

namespace Empty_ERP_Template.Business.Validators.PersonnelValidator;

public class PersonnelContactValidator : AbstractValidator<PersonnelContactRequest>
{
    public PersonnelContactValidator()
    {
        RuleFor(x => x.PersonnelId)
            .GreaterThan(0)
            .WithMessage("Personnel bilgisi seçilmelidir.");

        RuleFor(x => x.PersonalEmail)
            .EmailAddress().WithMessage("Kiþisel e-posta adresi geçerli formatta olmalýdýr.")
            .When(x => !string.IsNullOrEmpty(x.PersonalEmail));

        RuleFor(x => x.WorkEmail)
            .EmailAddress().WithMessage("Ýþ e-posta adresi geçerli formatta olmalýdýr.")
            .When(x => !string.IsNullOrEmpty(x.WorkEmail));

        RuleFor(x => x.MobilePhone)
            .MaximumLength(50).WithMessage("Mobil telefon en fazla 50 karakter olabilir.");

        RuleFor(x => x.HomePhone)
            .MaximumLength(50).WithMessage("Ev telefonu en fazla 50 karakter olabilir.");

        RuleFor(x => x.WorkPhone)
            .MaximumLength(50).WithMessage("Ýþ telefonu en fazla 50 karakter olabilir.");

        RuleFor(x => x.AddressLine1)
            .MaximumLength(250).WithMessage("Adres satýrý 1 en fazla 250 karakter olabilir.");

        RuleFor(x => x.AddressLine2)
            .MaximumLength(250).WithMessage("Adres satýrý 2 en fazla 250 karakter olabilir.");

        RuleFor(x => x.CityId)
            .MaximumLength(100).WithMessage("Þehir bilgisi en fazla 100 karakter olabilir.");

        RuleFor(x => x.StateId)
            .MaximumLength(100).WithMessage("Ýl bilgisi en fazla 100 karakter olabilir.");

        RuleFor(x => x.CountryId)
            .MaximumLength(100).WithMessage("Ülke bilgisi en fazla 100 karakter olabilir.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(20).WithMessage("Posta kodu en fazla 20 karakter olabilir.");
    }
}
