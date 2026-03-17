using Empty_ERP_Template.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.PersonnelConfiguration;

public class PersonnelConfiguration : IEntityTypeConfiguration<Personnel>
{
    public void Configure(EntityTypeBuilder<Personnel> builder)
    {
        builder.HasOne(p => p.GenderType)
            .WithMany()
            .HasForeignKey(p => p.GenderTypeId);

        builder.HasOne(p => p.MaritalStatusType)
                     .WithMany()
                     .HasForeignKey(p => p.MaritalStatusTypeId);

        builder.HasOne(p => p.DepartmentType)
               .WithMany()
               .HasForeignKey(p => p.DepartmentTypeId);

        builder.HasOne(p => p.PersonnelType)
               .WithMany()
               .HasForeignKey(p => p.PersonnelTypeId);

        builder.HasOne(p => p.StatusType)
               .WithMany()
               .HasForeignKey(p => p.StatusTypeId);
    }
}
