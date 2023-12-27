using Microsoft.Extensions.Caching.Memory;

namespace WonderfulCaptcha.Cache.InMemory;

public class InMemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _memoryCache;

    public InMemoryCacheProvider(IMemoryCache iMemoryCache)
    {
        _memoryCache = iMemoryCache;
    }

    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken)
        => _memoryCache.Get<T>(cacheKey);

    public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setter, TimeSpan expiration, CancellationToken cancellationToken)
        => await _memoryCache.GetOrCreateAsync(cacheKey, s => setter());

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        => _memoryCache.Remove(key);

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken)
        => _memoryCache.Set<T>(cacheKey, cacheValue, expiration);
}

