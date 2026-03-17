namespace Lupion.Data.Entities.TaskManagerEntities;

public class TaskItem : Entity<int>
{
    public string? TaskNumber { get; set; }
    public string? Description { get; set; }
    public DateTime TaskDate { get; set; }
    public DateTime? TargetDate { get; set; }
    public int StatusId { get; set; }
    public int? PriorityId { get; set; }
    public string? Comment { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsArchived { get; set; }
}
