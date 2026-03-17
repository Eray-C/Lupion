using Empty_ERP_Template.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.PersonnelConfiguration;

public class PayrollConfiguration : IEntityTypeConfiguration<PaidPayroll>
{
    public void Configure(EntityTypeBuilder<PaidPayroll> builder)
    {
        builder.HasIndex(p => new { p.Year, p.Month }).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}
