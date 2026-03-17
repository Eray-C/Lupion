
namespace Empty_ERP_Template.Business.Attributes;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RedisCacheAttribute : Attribute
{
    public string Key { get; }
    public int? DurationMinutes { get; }
    public string[] EntityNames { get; }

    public RedisCacheAttribute(string key, int? durationMinutes = null, params string[] entityNames)
    {
        Key = key;
        DurationMinutes = durationMinutes;
        EntityNames = entityNames ?? Array.Empty<string>();
    }
    public RedisCacheAttribute(string key, params string[] entityNames)
    {
        Key = key;
        DurationMinutes = null;
        EntityNames = entityNames ?? Array.Empty<string>();
    }
}
