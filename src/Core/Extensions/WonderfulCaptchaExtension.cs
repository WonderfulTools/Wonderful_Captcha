using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WonderfulCaptcha.Cache;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;
public static class CaptchaServiceCollectionExtensions
{
    public static void AddWonderfulCaptcha(this IServiceCollection services, IConfiguration configuration, Action<CaptchaOptions>? options = null)
    {
        services.AddTransient<IWonderfulCaptchaService, WonderfulCaptchaService>();
        services.AddTransient<IContentWriter, ContentWriter>();
        services.AddTransient<IFilterEngine, FilterEngine>();
        services.AddTransient<ICanvasBuilder, CanvasBuilder>();
        var captchaOptions = new CaptchaOptions();
        options?.Invoke(captchaOptions);



        if (captchaOptions.CryptoProvider == default) services.AddTransient<ICryptoProvider, SHA256CryptoEngine>();
        if (captchaOptions.TextProvider == default) services.AddTransient<ITextProvider, TextFactory>();
        if (captchaOptions.ImageGenerator == default) services.AddScoped<IImageGenerator, ImageGenerator>();


        services.AddScoped(s => captchaOptions);
        captchaOptions.CacheProvider = services.BuildServiceProvider().GetRequiredService<ICacheProvider>();
        captchaOptions.CryptoProvider = services.BuildServiceProvider().GetRequiredService<ICryptoProvider>();
        captchaOptions.TextProvider = services.BuildServiceProvider().GetRequiredService<ITextProvider>();
        captchaOptions.ImageGenerator = services.BuildServiceProvider().GetRequiredService<IImageGenerator>();
        services.AddScoped(s => captchaOptions);
        //var a = new CaptchaOptions
        //{
        //    Text = captchaOptions.Text,
        //    CacheExpirationTime = captchaOptions.CacheExpirationTime,
        //    CharPositionVarietyRange = captchaOptions.CharPositionVarietyRange,
        //    CharSpacing = captchaOptions.CharSpacing,
        //    Color = captchaOptions.Color,
        //    FontSize = captchaOptions.FontSize,
        //    FontSizeVarietyRange = captchaOptions.FontSizeVarietyRange,
        //    RelativeFitSizeThreshold = captchaOptions.RelativeFitSizeThreshold,
        //    Size = captchaOptions.Size,
        //    SizeStrategy = captchaOptions.SizeStrategy,
        //    Strategy = captchaOptions.Strategy,
        //    TextBrush = captchaOptions.TextBrush,
        //    TextColor = captchaOptions.TextColor,
        //    TextFont = captchaOptions.TextFont,
        //    TextFontStyle = captchaOptions.TextFontStyle,
        //    TextLen = captchaOptions.TextLen,
        //    TextRotationRange = captchaOptions.TextRotationRange,
        //    TextShadow = captchaOptions.TextShadow,
        //    TextSkewRange = captchaOptions.TextSkewRange,
        //    Noise = new NoiseOptions
        //    {
        //        Density = captchaOptions.Noise.Density,
        //    },
        //    Complexity = captchaOptions.Complexity,
        //    CacheProvider = captchaOptions.CacheProvider,
        //    CryptoProvider = captchaOptions.CryptoProvider,
        //    TextProvider = captchaOptions.TextProvider,
        //    ImageGenerator = captchaOptions.ImageGenerator
        //};
    }
}