namespace Lupion.Data.Entities.AuthenticationEntities;

public class ApiPermission
{
    public int Id { get; set; }
    public string Path { get; set; }
    public string Method { get; set; } 
    public int? ModuleOperationId { get; set; }
}
