namespace Cache;

public interface ICacheProvider
{
    Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken);
    Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setter, TimeSpan expiration, CancellationToken cancellationToken);
    Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken);
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}
