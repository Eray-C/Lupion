using Empty_ERP_Template.Data.Entities.CustomerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerPriceConfiguration : IEntityTypeConfiguration<CustomerPrice>
{
    public void Configure(EntityTypeBuilder<CustomerPrice> builder)
    {
    }
}
