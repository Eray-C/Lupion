using Empty_ERP_Template.Data.Entities.TaskManagerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empty_ERP_Template.Data.Configurations.TaskManager;

public class TaskConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        builder.Property(x => x.TaskNumber).HasMaxLength(64);
        builder.Property(x => x.Description).HasMaxLength(512);
        builder.Property(x => x.Comment).HasMaxLength(512);
        builder.Property(x => x.IsArchived).HasDefaultValue(false);
    }
}
