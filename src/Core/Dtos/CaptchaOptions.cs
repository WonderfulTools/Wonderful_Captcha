using Cache.implementations;

namespace WonderfulCaptcha;

public class CaptchaOptions
{
    internal StrategyEnum Strategy { get; set; } = StrategyEnum.Character;
    internal string Text { get; set; } = default!;
    internal (int Width, int Height) Size { get; set; } = (10, 10);
    internal (int Min, int Max) TextLen { get; set; } = (5, 7);
    internal ColorEnum Color { get; set; } = ColorEnum.Red;
    internal int FontSize { get; set; } = 25;
    internal Type? CacheProvider { get; set; }
    internal TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(1);
    internal NoiseOptions Noise { get; set; } = new NoiseOptions();

    #region methods

    public CaptchaOptions WithSize(int width = 10, int height = 20)
    {
        Size = (width, height);
        return this;
    }
    public CaptchaOptions WithColor(ColorEnum color)
    {
        Color = color;
        return this;
    }
    public CaptchaOptions WithTextLen(int min, int max)
    {
        TextLen = (min, max);
        return this;
    }
    public CaptchaOptions WithFontSize(int fontSize)
    {
        FontSize = fontSize;
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
    public CaptchaOptions UseInMemoryCacheProvider(TimeSpan? cacheExpirationTime = default!)
    {
        CacheProvider = typeof(InMemoryCacheProvider);
        if (cacheExpirationTime is not null)
            CacheExpirationTime = cacheExpirationTime ?? TimeSpan.FromMinutes(1);
        return this;
    }
    public CaptchaOptions UseRedisCacheProvider(TimeSpan? cacheExpirationTime = default!)
    {
        CacheProvider = typeof(RedisCacheProvider);
        if (cacheExpirationTime is not null)
            CacheExpirationTime = cacheExpirationTime ?? TimeSpan.FromMinutes(1);
        return this;
    }
    public CaptchaOptions UseEasyCachingCacheProvider(TimeSpan? cacheExpirationTime = default!)
    {
        CacheProvider = typeof(EasyCachingProvider);
        if (cacheExpirationTime is not null)
            CacheExpirationTime = cacheExpirationTime ?? TimeSpan.FromMinutes(1);
        return this;
    }
    public CaptchaOptions UseCacheExpirationTime(TimeSpan cacheExpirationTime)
    {
        CacheExpirationTime = cacheExpirationTime;
        return this;
    }

    #endregion
}