using Empty_ERP_Template.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.PersonnelConfiguration;

public class PersonnelAdvancePaymentConfiguration : IEntityTypeConfiguration<PersonnelAdvancePayment>
{
    public void Configure(EntityTypeBuilder<PersonnelAdvancePayment> builder)
    {
        builder.HasOne(p => p.PersonnelAdvance)
            .WithMany()
            .HasForeignKey(p => p.PersonnelAdvanceId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.PersonnelPayroll)
            .WithMany()
            .HasForeignKey(p => p.PersonnelPayrollId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
