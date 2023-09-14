
using EasyCaching.Core;

namespace Cache.implementations;

public class EasyCachingProvider : ICacheProvider
{
    private readonly IEasyCachingProvider _easyCachingProvider;

    public EasyCachingProvider(IEasyCachingProvider iEasyCachingProvider)
    {
        _easyCachingProvider = iEasyCachingProvider;
    }

    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken)
        => (await _easyCachingProvider.GetAsync<T>(cacheKey, cancellationToken)).Value;

    public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setter, TimeSpan expiration, CancellationToken cancellationToken)
        => (await _easyCachingProvider.GetAsync(cacheKey, setter, expiration, cancellationToken)).Value;

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        => await _easyCachingProvider.RemoveAsync(key, cancellationToken);

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken)
        => await _easyCachingProvider.SetAsync(cacheKey, cacheValue, expiration, cancellationToken);
}

