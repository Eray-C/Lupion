using Lupion.Business.Requests.Customer;
using FluentValidation;

namespace Lupion.Business.Validators.CustomerValidator;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("MÃ¼ÅŸteri adÄ± boÅŸ olamaz.")
            .MaximumLength(100).WithMessage("MÃ¼ÅŸteri adÄ± 100 karakterden uzun olamaz.");

        RuleFor(x => x.TaxNumber)
            .MaximumLength(50).WithMessage("Vergi numarasÄ± 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.TaxNumber));

        RuleFor(x => x.Address)
            .MaximumLength(250).WithMessage("Adres 250 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Ãœlke 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Country));

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("Åehir 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("Telefon 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("E-posta adresi geÃ§erli formatta olmalÄ±dÄ±r.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.BankName)
            .MaximumLength(100).WithMessage("Banka adÄ± 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.BankName));

        RuleFor(x => x.BankNumber)
            .MaximumLength(50).WithMessage("Banka numarasÄ± 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.BankNumber));

        RuleFor(x => x.BankAccountNo)
            .MaximumLength(50).WithMessage("Banka hesap numarasÄ± 50 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.BankAccountNo));
    }
}
