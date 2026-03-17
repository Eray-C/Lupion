using Lupion.Business.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Lupion.Business.Services;

public class RedisCacheService(IDistributedCache cache, TenantContext tenant)
{
    private string AddTenantPrefix(string key)
    {
        if (string.IsNullOrWhiteSpace(tenant.Tenant))
            return key;
        var prefix = $"CheetahCache:{tenant.Tenant}:";
        return key.StartsWith(prefix) ? key : $"{prefix}{key}";
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await cache.GetStringAsync(AddTenantPrefix(key));
        if (data == null)
            return default;

        if (typeof(T) == typeof(string))
            return (T)(object)data;

        if (data.StartsWith("\"") && data.EndsWith("\""))
        {
            var inner = JsonSerializer.Deserialize<string>(data);
            return JsonSerializer.Deserialize<T>(inner);
        }

        return JsonSerializer.Deserialize<T>(data);

    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        string json;

        if (value is string strValue)
            json = strValue;
        else
            json = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions();

        if (expiry.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiry.Value;

        await cache.SetStringAsync(AddTenantPrefix(key), json, options);
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(AddTenantPrefix(key));
    }

    public async Task RegisterKeyForEntityAsync(string entityName, string cacheKey)
    {
        var listKey = AddTenantPrefix($"CacheKeys:{entityName}");
        var db = ConnectionMultiplexer.Connect("194.62.55.36:6379,password=ErfaCheetahRedis?").GetDatabase();
        await db.SetAddAsync(listKey, cacheKey);
    }


    public async Task<IEnumerable<string>> GetKeysByEntityAsync(string entityName)
    {
        var listKey = AddTenantPrefix($"CacheKeys:{entityName}");
        var conn = ConnectionMultiplexer.Connect("194.62.55.36:6379,password=ErfaCheetahRedis?");
        var db = conn.GetDatabase();

        var members = await db.SetMembersAsync(listKey);
        if (members == null || members.Length == 0)
            return Enumerable.Empty<string>();

        return members.Select(m => (string)m);
    }


    public async Task RemoveByEntityAsync(string entityName)
    {
        var keys = await GetKeysByEntityAsync(entityName);
        foreach (var key in keys)
            await cache.RemoveAsync(AddTenantPrefix(key));
    }
}
