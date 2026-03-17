using Lupion.Business.Requests.Customer;
using FluentValidation;

namespace Lupion.Business.Validators.CustomerValidator;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Müşteri adı boş olamaz.")
            .MaximumLength(100).WithMessage("Müşteri adı 100 karakterden uzun olamaz.");

        RuleFor(x => x.TaxNumber)
            .MaximumLength(50).WithMessage("Vergi numarası 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.TaxNumber));

        RuleFor(x => x.Address)
            .MaximumLength(250).WithMessage("Adres 250 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Ülke 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Country));

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("Şehir 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("Telefon 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("E-posta adresi geçerli formatta olmalıdır.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.BankName)
            .MaximumLength(100).WithMessage("Banka adı 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.BankName));

        RuleFor(x => x.BankNumber)
            .MaximumLength(50).WithMessage("Banka numarası 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.BankNumber));

        RuleFor(x => x.BankAccountNo)
            .MaximumLength(50).WithMessage("Banka hesap numarası 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.BankAccountNo));
    }
}
