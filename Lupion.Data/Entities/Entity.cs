using Lupion.Data.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations;

namespace Lupion.Data.Entities;

public class Entity<T> : ISoftDeletable
{
    [Key]
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
}
