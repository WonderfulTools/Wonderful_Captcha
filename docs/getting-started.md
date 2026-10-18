# Getting started

## 1. Install

```bash
dotnet add package WonderfulTools.WonderfulCaptcha
# plus one cache provider:
dotnet add package WonderfulTools.WonderfulCaptcha.Cache.InMemory   # single instance
dotnet add package WonderfulTools.WonderfulCaptcha.Cache.Redis      # multiple instances / load balanced
```

## 2. Register

A cache provider is **required** — `AddWonderfulCaptcha` throws at startup if none is configured.

```csharp
using WonderfulCaptcha;
using WonderfulCaptcha.Cache.InMemory;

builder.Services.AddMemoryCache();
builder.Services.AddWonderfulCaptcha(o =>
{
    o.UseInMemoryCacheProvider();
    o.TextOptions.Strategy = StrategyEnum.Digits;
    o.CacheOptions.CacheExpirationTime = TimeSpan.FromMinutes(3);
});
```

Redis:

```csharp
using WonderfulCaptcha.Cache.Redis;

builder.Services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = "localhost:6379";
    o.InstanceName = "MyApp:";
});
builder.Services.AddWonderfulCaptcha(o => o.UseRedisCacheProvider());
```

## 3. Generate

```csharp
app.MapGet("/captcha", async (IWonderfulCaptchaService captcha, CancellationToken ct) =>
    await captcha.GenerateAsync(cancellationToken: ct));
// => { "key": "3f1c…", "image": "data:image/png;base64,iVBOR…" }
```

Put `image` straight into an `<img src>`; keep `key` in the form (hidden field).

Per-call overrides apply **to that captcha only** (since the race-condition fix they no longer leak into later requests):

```csharp
await captcha.GenerateAsync(o => o.TextOptions.TextLen = (6, 6), ct);
```

## 4. Verify

```csharp
bool ok = await captcha.VerifyAsync(key, userInput, ct);
```

- Case-insensitive comparison.
- **One-shot:** the key is deleted on the first verify attempt, right or wrong. After a failure the client **must fetch a new captcha**; re-submitting against the same key always returns `false`.
- Returns `false` if the key expired (default 1 minute) or never existed.

## Sample projects

- `Samples/sample.ApiClient` — ASP.NET Core API with Swagger (`/Test/Generate`, `/Test/Verify?key=&value=`) and `captchatester.html`.
- `Samples/sample.NugetAlpine` — minimal API on Alpine, consuming the NuGet package, returns the PNG at `/captcha`.
