using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WonderfulCaptcha.Cache;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;

public class CaptchaOptions
{
    internal StrategyEnum Strategy { get; set; } = StrategyEnum.Character;
    internal string Text { get; set; } = default!;
    internal ColorEnum TextColor { get; set; } = ColorEnum.Random;
    internal string TextColorHex { get; set; } = default!;
    internal FontEnum TextFont { get; set; } = FontEnum.Random;
    internal FontStyleEnum TextFontStyle { get; set; } = FontStyleEnum.Random;
    internal BrushEnum TextBrush { get; set; } = BrushEnum.Random;
    internal (int Min, int Max) TextLen { get; set; } = (5, 5);
    internal int FontSize { get; set; } = 50;
    internal int FontSizeVarietyRange { get; set; } = 5;
    internal bool TextShadow { get; set; } = true;
    internal int TextRotationRange { get; set; } = 12;
    internal int TextSkewRange { get; set; } = 10;

    internal (int Width, int Height) ImageSize { get; set; } = (350, 100);
    internal ImageSizeStrategy ImageSizeStrategy { get; set; } = ImageSizeStrategy.Dynamic;
    internal ColorEnum ImageBackgroundColor { get; set; } = ColorEnum.White;
    internal string ImageBackgroundColorHex { get; set; } = default!;
    internal int RelativeFitSizeThreshold { get; set; } = 25;
    internal int CharSpacing { get; set; } = 10;
    internal (int Width, int Height) CharPositionVarietyRange { get; set; } = (15, 15);
    internal int Complexity { get; set; } = 0;

    public ICacheProvider CacheProvider { get; set; } = default!;
    internal TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(1);

    internal ICryptoProvider CryptoProvider { get; set; } = default!;
    internal ITextProvider TextProvider { get; set; } = default!;
    internal IImageGenerator ImageGenerator { get; set; } = default!;
    internal NoiseOptions Noise { get; set; } = new NoiseOptions();


    #region methods

    public CaptchaOptions WithTextColor(ColorEnum color) { TextColor = color; return this; }
    public CaptchaOptions WithTextColor(string colorHex) { TextColorHex = colorHex; return this; }
    public CaptchaOptions WithTextFont(FontEnum font) { TextFont = font; return this; }
    public CaptchaOptions WithTextFontStyle(FontStyleEnum fontStyle) { TextFontStyle = fontStyle; return this; }
    public CaptchaOptions WithTextBrush(BrushEnum brush) { TextBrush = brush; return this; }
    public CaptchaOptions WithTextLength(int min, int max) { TextLen = (min, max); return this; }
    public CaptchaOptions WithTextFontSize(int fontSize) { FontSize = fontSize; return this; }
    public CaptchaOptions WithTextFontSizeVarietyRange(int varietyRange) { FontSizeVarietyRange = varietyRange; return this; }
    public CaptchaOptions WithTextShadow(bool haveShadow = true) { TextShadow = haveShadow; return this; }
    public CaptchaOptions WithTextRotationRange(int textRotationRange) { TextRotationRange = textRotationRange; return this; }
    public CaptchaOptions WithTextSkewRange(int textSkewRange) { TextSkewRange = textSkewRange; return this; }


    public CaptchaOptions WithImageSize(int width, int height) { ImageSize = (width, height); return this; }
    public CaptchaOptions WithImageSizeStrategy(ImageSizeStrategy imageSizeStrategy) { ImageSizeStrategy = imageSizeStrategy; return this; }
    public CaptchaOptions WithImageBackgroundColor(ColorEnum color) { ImageBackgroundColor = color; return this; }
    public CaptchaOptions WithImageBackgroundColorHex(string colorHex) { ImageBackgroundColorHex = colorHex; return this; }
    public CaptchaOptions WithRelativeFitSizeThreshold(int relativeFitSizeThreshold) { RelativeFitSizeThreshold = relativeFitSizeThreshold; return this; }
    public CaptchaOptions WithCharSpacing(int charSpacing) { CharSpacing = charSpacing; return this; }
    public CaptchaOptions WithCharPositionVarietyRange(int width, int height) { CharPositionVarietyRange = (width, height); return this; }



    public CaptchaOptions WithComplexity(int complexity) { Complexity = complexity; return this; }
    public CaptchaOptions WithStrategy(StrategyEnum strategy) { Strategy = strategy; return this; }


    public CaptchaOptions UseCacheProvider<TProvider>(IServiceCollection services, TimeSpan? cacheExpirationTime = default!) where TProvider : class, ICacheProvider
    {
        services.TryAddScoped<ICacheProvider, TProvider>();
        return this;
    }
    public CaptchaOptions UseCacheExpirationTime(TimeSpan cacheExpirationTime)
    {
        CacheExpirationTime = cacheExpirationTime;
        return this;
    }

    public CaptchaOptions UseCryptoProvider<TProvider>(IServiceCollection services) where TProvider : class, ICryptoProvider
    {
        services.TryAddScoped<ICryptoProvider, TProvider>();
        return this;
    }
    public CaptchaOptions UseTextProvider<TProvider>(IServiceCollection services) where TProvider : class, ITextProvider
    {
        services.TryAddScoped<ITextProvider, TProvider>();
        return this;
    }
    public CaptchaOptions UseIImageGenerator<TProvider>(IServiceCollection services) where TProvider : class, IImageGenerator
    {
        services.TryAddScoped<IImageGenerator, TProvider>();
        return this;
    }



    public CaptchaOptions UseNoise(NoiseOptions noiseOptions)
    {
        Noise = noiseOptions;
        return this;
    }


    #endregion
}