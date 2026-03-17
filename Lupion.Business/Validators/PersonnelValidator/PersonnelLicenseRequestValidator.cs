using Lupion.Business.Requests.Personnel;
using FluentValidation;

namespace Lupion.Business.Validators.PersonnelValidator;

public class PersonnelLicenseRequestValidator : AbstractValidator<PersonnelLicenseRequest>
{
    public PersonnelLicenseRequestValidator()
    {
        RuleFor(x => x.PersonnelId)
            .GreaterThan(0).WithMessage("Personel seçilmelidir");

        RuleFor(x => x.LicenseTypeId)
            .GreaterThan(0).WithMessage("Ehliyet/sertifika tipi seçilmelidir");

        RuleFor(x => x.LicenseNumber)
            .MaximumLength(50).WithMessage("Ehliyet numarası 50 karakterden uzun olamaz");

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("Kategori 50 karakterden uzun olamaz");

        RuleFor(x => x.IssuedBy)
            .MaximumLength(150).WithMessage("Veren kurum bilgisi 150 karakterden uzun olamaz");

        RuleFor(x => x.IssueDate)
            .LessThanOrEqualTo(DateTime.Today)
            .When(x => x.IssueDate.HasValue)
            .WithMessage("Veriliş tarihi bugünden büyük olamaz");

        RuleFor(x => x.ExpiryDate)
            .GreaterThanOrEqualTo(x => x.IssueDate)
            .When(x => x.IssueDate.HasValue && x.ExpiryDate.HasValue)
            .WithMessage("Bitiş tarihi veriliş tarihinden önce olamaz");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notlar 1000 karakterden uzun olamaz");
    }
}
