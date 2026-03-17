using Lupion.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lupion.Data.Configurations.PersonnelConfiguration;

public class PersonnelPaymentHistoryConfiguration : IEntityTypeConfiguration<PersonnelPaymentHistory>
{
    public void Configure(EntityTypeBuilder<PersonnelPaymentHistory> builder)
    {
        builder.HasOne(p => p.PersonnelPayroll)
            .WithMany()
            .HasForeignKey(p => p.PersonnelPayrollId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
