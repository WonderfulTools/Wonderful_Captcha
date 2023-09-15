using Cache.implementations;

namespace WonderfulCaptcha;
public class CaptchaOptions
{
    internal string Text { get; set; } = default!;
    internal (int height, int width) Size { get; set; } = (10, 20);
    internal StrategyEnum Strategy { get; set; } = StrategyEnum.Digits;
    internal (int Min, int Max) TextLen { get; set; } = (5, 7);
    internal Type? CacheProvider { get; set; }
    internal TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(1);



    public CaptchaOptions WithSize(int height = 10, int width = 20)
    {
        Size = (height, width);
        return this;
    }
    public CaptchaOptions WithCaptchaText(string text)
    {
        Text = text;
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
}
