namespace Empty_ERP_Template.Data.Entities.TaskManagerEntities;

public class TaskHistory : Entity<int>
{
    public int TaskId { get; set; }
    public int? OldStatusId { get; set; }
    public int NewStatusId { get; set; }
    public int ChangedBy { get; set; }
    public DateTime ChangedDate { get; set; }
    public string? Comment { get; set; }
}
