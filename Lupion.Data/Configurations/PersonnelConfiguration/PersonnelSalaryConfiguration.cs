using Lupion.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lupion.Data.Configurations.PersonnelConfiguration;

public class PersonnelSalaryConfiguration : IEntityTypeConfiguration<PersonnelSalary>
{
    public void Configure(EntityTypeBuilder<PersonnelSalary> builder)
    {
        builder.HasOne(s => s.Currency)
            .WithMany()
            .HasForeignKey(s => s.CurrencyCode)
            .HasPrincipalKey(c => c.Code);

        builder.HasOne(s => s.PaymentType)
            .WithMany()
            .HasForeignKey(s => s.PaymentTypeId)
            .HasPrincipalKey(c => c.Id);

    }
}

