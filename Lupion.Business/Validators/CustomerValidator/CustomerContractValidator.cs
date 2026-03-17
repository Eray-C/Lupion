using Lupion.Business.Requests.Customer;
using FluentValidation;

namespace Lupion.Business.Validators.CustomerValidator;

public class CustomerContractValidator : AbstractValidator<CustomerContractRequest>
{
    public CustomerContractValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.ContractNumber)
            .MaximumLength(100).WithMessage("SÃ¶zleÅŸme numarasÄ± 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.ContractNumber));

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("BitiÅŸ tarihi baÅŸlangÄ±Ã§ tarihinden bÃ¼yÃ¼k olmalÄ±dÄ±r.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("AÃ§Ä±klama 1000 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.FreightTerms)
            .MaximumLength(500).WithMessage("Navlun ÅŸartlarÄ± 500 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.FreightTerms));

        RuleFor(x => x.PriceList)
            .MaximumLength(500).WithMessage("Fiyat listesi 500 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.PriceList));
    }
}
