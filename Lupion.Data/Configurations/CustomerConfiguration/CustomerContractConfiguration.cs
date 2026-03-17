using Lupion.Data.Entities.CustomerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lupion.Data.Configurations.CustomerConfiguration;

public class CustomerContractConfiguration() : IEntityTypeConfiguration<CustomerContract>
{
    public void Configure(EntityTypeBuilder<CustomerContract> builder)
    {
    }
}
