using Microsoft.Extensions.Caching.Memory;
using WonderfulCaptcha;
using WonderfulCaptcha.Cache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddWonderfulCaptcha(o => o.UseCustomCacheProvider<MemoryCacheProvider>());

var app = builder.Build();

// Returns the captcha as a real PNG so it can be opened in a browser.
app.MapGet("/captcha", async (IWonderfulCaptchaService captcha, CancellationToken ct) =>
{
    var result = await captcha.GenerateAsync(cancellationToken: ct);
    var base64 = result.Image[(result.Image.IndexOf(',') + 1)..];
    return Results.File(Convert.FromBase64String(base64), "image/png");
});

app.Run();

// ponytail: published Cache.InMemory 10.2.4 ships Core.dll in lib/net10.0; switch back to it once republished.
class MemoryCacheProvider(IMemoryCache cache) : ICacheProvider
{
    public Task<T> GetAsync<T>(string key, CancellationToken ct = default) => Task.FromResult(cache.Get<T>(key)!);
    public Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken ct = default)
    { cache.Set(key, value, expiration); return Task.CompletedTask; }
    public async Task<T> GetAsync<T>(string key, Func<Task<T>> setter, TimeSpan expiration, CancellationToken ct = default)
        => (await cache.GetOrCreateAsync(key, e => { e.AbsoluteExpirationRelativeToNow = expiration; return setter(); }))!;
    public Task RemoveAsync(string key, CancellationToken ct = default) { cache.Remove(key); return Task.CompletedTask; }
}
