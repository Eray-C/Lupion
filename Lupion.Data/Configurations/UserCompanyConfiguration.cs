using Lupion.Data.Entities.AuthenticationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lupion.Data.Configurations;

public class UserCompanyConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(u => u.Company)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.CompanyCode);
    }
}