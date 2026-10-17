# Cache providers

The captcha answer is AES-encrypted and stored under `{Prefix}_{key}` with an absolute TTL of `CacheOptions.CacheExpirationTime`. Verification reads it, **deletes it**, then compares.

The provider type is registered as a **singleton** (`TryAddSingleton`), so its dependencies must be singleton-safe (`IMemoryCache`, `IDistributedCache`, `IEasyCachingProvider` all are).

## InMemory — `WonderfulTools.WonderfulCaptcha.Cache.InMemory`

```csharp
builder.Services.AddMemoryCache();
builder.Services.AddWonderfulCaptcha(o => o.UseInMemoryCacheProvider());
```

> ⚠️ **Single instance only.** With more than one app instance (Kubernetes replicas, IIS web garden, load balancer, multiple containers) a captcha generated on instance A is unknown to instance B. Verification then succeeds only when both requests happen to hit the same instance — it looks like "random" success after several tries. Use Redis (or sticky sessions) in that case.

## Redis — `WonderfulTools.WonderfulCaptcha.Cache.Redis`

Works with any `IDistributedCache`; values are JSON-serialized (Newtonsoft).

```csharp
builder.Services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = "localhost:6379";
    o.InstanceName = "MyApp:";          // prepended by IDistributedCache
});
builder.Services.AddWonderfulCaptcha(o => o.UseRedisCacheProvider());
```

Resulting Redis key: `{InstanceName}{Prefix}_{guid}`, e.g. `MyApp:WonderfulCaptcha_3f1c…`. All instances must use the same `InstanceName` and `Prefix`.

Debugging: `redis-cli --scan --pattern '*WonderfulCaptcha_*'` and `redis-cli TTL <key>` (`-2` = gone/expired).

## EasyCaching — `src/Cache.EasyCaching`

```csharp
builder.Services.AddEasyCaching(o => o.UseInMemory());   // or UseRedis(...)
builder.Services.AddWonderfulCaptcha(o => o.UseEasyCachingProvider());
```

Not published to NuGet yet (no `PackageId`); reference the project directly.

## Custom provider

Implement `WonderfulCaptcha.Cache.ICacheProvider` and register with `o.UseCustomCacheProvider<MyProvider>()`. See [extending.md](extending.md#custom-cache-provider).
