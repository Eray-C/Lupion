using Lupion.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lupion.Data.Configurations.PersonnelConfiguration;

public class PersonnelDeductionConfiguration : IEntityTypeConfiguration<PersonnelDeduction>
{
    public void Configure(EntityTypeBuilder<PersonnelDeduction> builder)
    {
        builder.HasOne(d => d.Type)
            .WithMany()
            .HasForeignKey(d => d.TypeId);
        builder.HasOne(d => d.Currency)
            .WithMany()
            .HasForeignKey(d => d.CurrencyCode)
            .HasPrincipalKey(c => c.Code);
    }
}
