using Lupion.Data.Entities.AuthenticationEntities;
using System.ComponentModel.DataAnnotations;

namespace Lupion.Data.Entities;

public class Company : Entity<int>
{
    [Key]
    public string Code { get; set; }
    public new int Id { get; set; }
    public string Name { get; set; }
    public string ConnectionString { get; set; }
    public ICollection<User> Users { get; set; } = [];
}
