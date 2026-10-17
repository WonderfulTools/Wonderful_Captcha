# Extending Wonderful Captcha

## Custom cache provider

```csharp
using WonderfulCaptcha.Cache;

public class MyCacheProvider : ICacheProvider
{
    public Task<T> GetAsync<T>(string cacheKey, CancellationToken ct = default) { … }   // return default when missing
    public Task SetAsync<T>(string cacheKey, T value, TimeSpan expiration, CancellationToken ct = default) { … }
    public Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setter, TimeSpan expiration, CancellationToken ct = default) { … }
    public Task RemoveAsync(string key, CancellationToken ct = default) { … }
}

builder.Services.AddWonderfulCaptcha(o => o.UseCustomCacheProvider<MyCacheProvider>());
```

Requirements:
- Registered as a **singleton** — must be thread-safe and must not depend on scoped services.
- **Honour `expiration`** as an absolute TTL (the sample `BehdadProvider`/InMemory provider pass it to `IMemoryCache.Set`).
- `GetAsync` must return `null`/`default` for a missing key — not throw.
- If the app runs on several instances, the provider must be shared (distributed).

## Custom crypto

Replace `ICryptoProvider` after `AddWonderfulCaptcha`:

```csharp
builder.Services.AddScoped<ICryptoProvider, MyCrypto>();   // last registration wins
```

`Decrypt(Encrypt(x))` must equal `x`. The default engine uses a hard-coded key; replace it if the cached value must stay confidential from people with cache access.

## Custom text strategy

Implement `ITextStrategy` and supply an `ITextProvider` that returns it:

```csharp
public class WordStrategy : ITextStrategy
{
    public TextProviderResultDto GetText(int len) => new("apple", "apple");
}

public class MyTextProvider : ITextProvider
{
    private readonly TextFactory _default = new();
    public ITextStrategy GetInstance(StrategyEnum type)
        => type == StrategyEnum.Character ? new WordStrategy() : _default.GetInstance(type);
}

builder.Services.AddScoped<ITextProvider, MyTextProvider>();
```

Use characters that are unambiguous when distorted and remember comparison is case-insensitive.

## Custom image pipeline (breaking change in v-next)

`ICanvasBuilder.Create`, `IContentWriter.WriteText`, `IFilterEngine.ApplyFilters` and `IImageGenerator.GenerateImageAsync` now take the per-call `CaptchaOptions` as a parameter. Implementations must read options **only** from that parameter — never from `IOptions<CaptchaOptions>` — and keep no per-call state in fields.

```csharp
public class NoNoiseFilterEngine : IFilterEngine
{
    public void ApplyFilters(Image<Rgba32> image, CaptchaOptions options) { /* nothing */ }
}
builder.Services.AddScoped<IFilterEngine, NoNoiseFilterEngine>();
```
