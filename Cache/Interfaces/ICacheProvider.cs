namespace Cache;

public interface ICacheProvider
{
    Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default!);
    Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default!);
    Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setter, TimeSpan expiration, CancellationToken cancellationToken = default!);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default!);
}
