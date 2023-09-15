using Cache.implementations;

namespace WonderfulCaptcha;
public class CaptchaOptions
{
    internal string Text { get; set; } = default!;
    internal (int Width, int Height) Size { get; set; } = (35, 100);
    internal StrategyEnum Strategy { get; set; } = StrategyEnum.Digits;
    internal (int Min, int Max) TextLen { get; set; } = (5, 7);
    internal Type? CacheProvider { get; set; }
    internal TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(1);


    public CaptchaOptions WithSize(int width = 10, int height = 20)
    {
        Size = (width, height);
        return this;
    }
    public CaptchaOptions WithCaptchaText(string text)
    {
        Text = text;
        return this;
    }
    public CaptchaOptions WithTextLength(int min, int max)
    {
        TextLen = (min, max);
        return this;
    }
    public CaptchaOptions WithStrategy(StrategyEnum strategy)
    {
        Strategy = strategy;
        return this;
    }
    public CaptchaOptions UseInMemoryCacheProvider()
    {
        CacheProvider = typeof(InMemoryCacheProvider);
        return this;
    }
    public CaptchaOptions UseRedisCacheProvider()
    {
        CacheProvider = typeof(RedisCacheProvider);
        return this;
    }
    public CaptchaOptions UseEasyCachingCacheProvider()
    {
        CacheProvider = typeof(EasyCachingProvider);
        return this;
    }
    public CaptchaOptions UseCacheExpirationTime(TimeSpan cacheExpirationTime)
    {
        CacheExpirationTime = cacheExpirationTime;
        return this;
    }
}
