using Cache.implementations;

namespace WonderfulCaptcha;
public class CaptchaOptions
{
    public string Text { get; set; } = default!;
    public (int height, int width) Size { get; set; } = (10, 20);
    public Type? StrategyProvider { get; set; }
    public Type? CacheProvider { get; set; }

    public CaptchaOptions WithDigitStrategy()
    {
        StrategyProvider = typeof(object);
        return this;
    }
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
