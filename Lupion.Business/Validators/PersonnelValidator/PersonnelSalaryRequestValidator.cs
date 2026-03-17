using Lupion.Business.Requests.Personnel;
using FluentValidation;

namespace Lupion.Business.Validators.PersonnelValidator
{
    public class PersonnelSalaryRequestValidator : AbstractValidator<PersonnelSalaryRequest>
    {
        public PersonnelSalaryRequestValidator()
        {
            RuleFor(x => x.PersonnelId)
                .GreaterThan(0).WithMessage("Personel seÃ§ilmelidir.");

            //RuleFor(x => x.EffectiveDate)
            //    .NotEmpty().WithMessage("GeÃ§erlilik tarihi boÅŸ olamaz.")
            //    .LessThanOrEqualTo(DateTime.Today).WithMessage("GeÃ§erlilik tarihi bugÃ¼nden ileri olamaz.");

            //RuleFor(x => x.BaseSalary)
            //    .GreaterThan(0).WithMessage("Temel maaÅŸ sÄ±fÄ±rdan bÃ¼yÃ¼k olmalÄ±dÄ±r.");

            RuleFor(x => x.BankName)
                .MaximumLength(50).WithMessage("Banka adÄ± 50 karakterden uzun olamaz.");


            RuleFor(x => x.BankAccount)
                .MaximumLength(100).WithMessage("Banka hesabÄ± 100 karakterden uzun olamaz.");

            RuleFor(x => x.MealCardFee)
                .GreaterThanOrEqualTo(0).When(x => x.MealCardFee.HasValue).WithMessage("Yemek kartÄ± Ã¼creti negatif olamaz.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notlar 1000 karakterden uzun olamaz.");
        }
    }
}
