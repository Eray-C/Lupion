using System.ComponentModel.DataAnnotations;

namespace Lupion.Data.Entities;

public class AuditLog
{
    [Key]
    public int Id { get; set; }
    public string TableName { get; set; }
    public string RecordId { get; set; }
    public string Operation { get; set; }
    public string OperatorId { get; set; }
    public DateTime OperationDate { get; set; }
    public string OldData { get; set; }
    public string NewData { get; set; }
}
