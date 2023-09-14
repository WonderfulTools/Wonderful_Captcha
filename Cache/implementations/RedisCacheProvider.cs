using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Cache.implementations;

public class RedisCacheProvider : ICacheProvider
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheProvider(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken)
    {
        var cachedItem = await _distributedCache.GetAsync(cacheKey, cancellationToken);
        return Deserialize<T>(cachedItem);
    }

    public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setter, TimeSpan expiration, CancellationToken cancellationToken)
    {
        var cachedItem = await _distributedCache.GetAsync(cacheKey);
        if (cachedItem != null)
            return Deserialize<T>(cachedItem);
        else
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            var cacheValue = await setter();
            await _distributedCache.SetAsync(cacheKey, Serialize(cacheValue), cacheOptions, cancellationToken);
            return cacheValue;
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        => await _distributedCache.RemoveAsync(key, cancellationToken);

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        await _distributedCache.SetAsync(cacheKey, Serialize(cacheValue), cacheOptions, cancellationToken);
    }

    private T Deserialize<T>(byte[] data)
    {
        if (data == null)
        {
            return default(T);
        }

        var json = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(json);
    }
    private byte[] Serialize<T>(T item)
    {
        if (item == null)
        {
            return null;
        }

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
        return Encoding.UTF8.GetBytes(json);
    }
}

