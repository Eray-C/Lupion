using Lupion.Business.Requests.Personnel;
using FluentValidation;

namespace Lupion.Business.Validators.PersonnelValidator;

public class PersonnelAdvancePaymentRequestValidator : AbstractValidator<PersonnelAdvancePaymentRequest>
{
    public PersonnelAdvancePaymentRequestValidator()
    {
        RuleFor(x => x.PersonnelId).GreaterThan(0);
        RuleFor(x => x.PersonnelAdvanceId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PeriodYear).InclusiveBetween(2000, 2100);
        RuleFor(x => x.PeriodMonth).InclusiveBetween(1, 12);
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}
