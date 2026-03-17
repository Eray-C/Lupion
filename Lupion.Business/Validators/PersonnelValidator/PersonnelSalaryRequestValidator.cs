using Empty_ERP_Template.Business.Requests.Personnel;
using FluentValidation;

namespace Empty_ERP_Template.Business.Validators.PersonnelValidator
{
    public class PersonnelSalaryRequestValidator : AbstractValidator<PersonnelSalaryRequest>
    {
        public PersonnelSalaryRequestValidator()
        {
            RuleFor(x => x.PersonnelId)
                .GreaterThan(0).WithMessage("Personel seçilmelidir.");

            //RuleFor(x => x.EffectiveDate)
            //    .NotEmpty().WithMessage("Geçerlilik tarihi boş olamaz.")
            //    .LessThanOrEqualTo(DateTime.Today).WithMessage("Geçerlilik tarihi bugünden ileri olamaz.");

            //RuleFor(x => x.BaseSalary)
            //    .GreaterThan(0).WithMessage("Temel maaş sıfırdan büyük olmalıdır.");

            RuleFor(x => x.BankName)
                .MaximumLength(50).WithMessage("Banka adı 50 karakterden uzun olamaz.");


            RuleFor(x => x.BankAccount)
                .MaximumLength(100).WithMessage("Banka hesabı 100 karakterden uzun olamaz.");

            RuleFor(x => x.MealCardFee)
                .GreaterThanOrEqualTo(0).When(x => x.MealCardFee.HasValue).WithMessage("Yemek kartı ücreti negatif olamaz.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notlar 1000 karakterden uzun olamaz.");
        }
    }
}
