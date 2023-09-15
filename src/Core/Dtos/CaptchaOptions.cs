using Cache.implementations;

namespace WonderfulCaptcha;
public class CaptchaOptions
{
    public string Text { get; set; } = default!;
    public (int height, int width) Size { get; set; } = (10, 20);
    public StrategyEnum Strategy { get; set; } = StrategyEnum.Digits;
    public (int Min, int Max) TextLen { get; set; } = (3, 3);
    public Type? CacheProvider { get; set; }

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
