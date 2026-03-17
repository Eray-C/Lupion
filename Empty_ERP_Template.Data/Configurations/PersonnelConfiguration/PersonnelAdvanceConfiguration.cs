using Empty_ERP_Template.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.PersonnelConfiguration;

public class PersonnelAdvanceConfiguration : IEntityTypeConfiguration<PersonnelAdvance>
{
    public void Configure(EntityTypeBuilder<PersonnelAdvance> builder)
    {
        builder.HasOne(a => a.Currency)
            .WithMany()
            .HasForeignKey(a => a.CurrencyCode)
            .HasPrincipalKey(c => c.Code);
    }
}
