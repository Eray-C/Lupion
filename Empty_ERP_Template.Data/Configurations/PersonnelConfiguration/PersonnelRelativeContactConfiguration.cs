using Empty_ERP_Template.Data.Entities.PersonnelEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.PersonnelConfiguration;

public class PersonnelRelativeContactConfiguration : IEntityTypeConfiguration<PersonnelRelativeContact>
{
    public void Configure(EntityTypeBuilder<PersonnelRelativeContact> builder)
    {

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.RelationshipType)
               .WithMany()
               .HasForeignKey(x => x.RelationshipTypeId);

        builder.HasOne(x => x.GenderType)
               .WithMany()
               .HasForeignKey(x => x.GenderTypeId);
    }
}
