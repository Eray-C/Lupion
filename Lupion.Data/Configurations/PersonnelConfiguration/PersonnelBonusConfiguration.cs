using Lupion.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lupion.Data.Configurations.PersonnelConfiguration;

public class PersonnelBonusConfiguration : IEntityTypeConfiguration<PersonnelBonus>
{
    public void Configure(EntityTypeBuilder<PersonnelBonus> builder)
    {
        builder.HasOne(b => b.Type)
            .WithMany()
            .HasForeignKey(b => b.TypeId);
        builder.HasOne(b => b.Currency)
            .WithMany()
            .HasForeignKey(b => b.CurrencyCode)
            .HasPrincipalKey(c => c.Code);
    }
}
