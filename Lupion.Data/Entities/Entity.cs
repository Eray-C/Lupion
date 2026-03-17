using Empty_ERP_Template.Data.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations;

namespace Empty_ERP_Template.Data.Entities;

public class Entity<T> : ISoftDeletable
{
    [Key]
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
}
