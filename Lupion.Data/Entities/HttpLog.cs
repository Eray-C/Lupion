namespace Lupion.Data.Entities;
public class HttpLog
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public string? UserFullName { get; set; }
    public string? CompanyCode { get; set; }
    public string? ClientIp { get; set; }
    public string? Method { get; set; }
    public string? Endpoint { get; set; }
    public int StatusCode { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public long DurationMs { get; set; }
    public string? CurlCommand { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

