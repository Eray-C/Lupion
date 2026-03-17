using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Lupion.Business.Services;

public class CacheService(IMemoryCache memoryCache, IConfiguration configuration)
{
    private readonly MemoryCacheEntryOptions defaultOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(int.Parse(configuration["CacheSettings:AbsoluteExpirationRelativeToNow"] ?? "5"))
    };

    public async Task ClearAsync(string key)
    {
        memoryCache.Remove(key);
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> factory, MemoryCacheEntryOptions? options = null)
    {
        return await memoryCache.GetOrCreateAsync(key, factory, options ?? defaultOptions);
    }
}
