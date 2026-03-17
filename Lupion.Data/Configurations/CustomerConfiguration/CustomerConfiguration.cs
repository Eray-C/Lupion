using Empty_ERP_Template.Data.Entities.CustomerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.CustomerConfiguration;

public class CustomerConfiguration() : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {

    }
}
