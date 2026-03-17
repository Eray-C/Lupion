using Empty_ERP_Template.Data.Entities.CustomerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.CustomerConfiguration;

public class CustomerContractConfiguration() : IEntityTypeConfiguration<CustomerContract>
{
    public void Configure(EntityTypeBuilder<CustomerContract> builder)
    {
    }
}
