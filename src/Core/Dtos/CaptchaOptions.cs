using Cache.implementations;

namespace WonderfulCaptcha;

public class CaptchaOptions
{
    internal (int Width, int Height) Size { get; set; } = (350, 100);
    internal SizeStrategy SizeStrategy { get; set; } = SizeStrategy.Dynamic;
    internal int RelativeFitSizeThreshold { get; set; } = 25;
    internal int CharSpacing { get; set; } = 10;
    internal (int Width, int Height) CharPositionVarietyRange { get; set; } = (15, 15);
    internal (int Min, int Max) TextLen { get; set; } = (4, 6);
    internal int FontSize { get; set; } = 50;
    internal int FontSizeVarietyRange { get; set; } = 5;


    internal StrategyEnum Strategy { get; set; } = StrategyEnum.Character;
    internal string Text { get; set; } = default!;


    internal ColorEnum TextColor { get; set; } = ColorEnum.Random;
    internal FontEnum TextFont { get; set; } = FontEnum.Random;
    internal FontStyleEnum TextFontStyle { get; set; } = FontStyleEnum.Random;
    internal BrushEnum TextBrush { get; set; } = BrushEnum.Random;
    internal bool TextShadow { get; set; } = true;
    internal int TextRotationRange { get; set; } = 12;
    internal int TextSkewRange { get; set; } = 10;


    internal ColorEnum Color { get; set; } = ColorEnum.Red;

    internal int Complexity { get; set; } = 0;
    
    
    internal TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(1);
    internal NoiseOptions Noise { get; set; } = new NoiseOptions();

    internal Type? CacheProvider { get; set; }

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
    public CaptchaOptions WithComplexity(int complexity)
    {
        Complexity = complexity;
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