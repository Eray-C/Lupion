namespace Lupion.Business.Requests.TaskManager;

public class TaskRequest
{
    public int? Id { get; set; }
    public string? TaskNumber { get; set; }
    public string? Description { get; set; }
    public DateTime TaskDate { get; set; }
    public DateTime? TargetDate { get; set; }
    public int StatusId { get; set; }
    public int? PriorityId { get; set; }
    public string? Comment { get; set; }
}