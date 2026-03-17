using Lupion.Data.Entities.TaskManagerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lupion.Data.Configurations.TaskManager;

public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
{
    public void Configure(EntityTypeBuilder<TaskHistory> builder)
    {
        builder.ToTable("TaskHistories");
        builder.Property(x => x.Comment).HasMaxLength(512);
    }
}
