using Empty_ERP_Template.Business.Requests.Customer;
using FluentValidation;

namespace Empty_ERP_Template.Business.Validators.CustomerValidator;

public class CustomerContractValidator : AbstractValidator<CustomerContractRequest>
{
    public CustomerContractValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.ContractNumber)
            .MaximumLength(100).WithMessage("Sözleşme numarası 100 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.ContractNumber));

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Bitiş tarihi başlangıç tarihinden büyük olmalıdır.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Açıklama 1000 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.FreightTerms)
            .MaximumLength(500).WithMessage("Navlun şartları 500 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.FreightTerms));

        RuleFor(x => x.PriceList)
            .MaximumLength(500).WithMessage("Fiyat listesi 500 karakterden uzun olamaz.")
            .When(x => !string.IsNullOrEmpty(x.PriceList));
    }
}
