using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WonderfulCaptcha.Cache;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;
public static class CaptchaServiceCollectionExtensions
{
    public static IServiceCollection AddWonderfulCaptcha(this IServiceCollection services, Action<CaptchaOptions>? options = null)
    {
        services.AddScoped<IWonderfulCaptchaService, WonderfulCaptchaService>();
        services.AddScoped<IContentWriter, ContentWriter>();
        services.AddScoped<IFilterEngine, FilterEngine>();
        services.AddScoped<ICanvasBuilder, CanvasBuilder>();
        services.AddScoped<ICryptoProvider, SHA256CryptoEngine>();
        services.AddScoped<ITextProvider, TextFactory>();
        services.AddScoped<IImageGenerator, ImageGenerator>();

        var captchaOptions = new CaptchaOptions();
        options.Invoke(captchaOptions);

        services.TryAddSingleton(typeof(ICacheProvider), captchaOptions.CacheOptions.CacheProvider);


        services.Configure(options ?? (o => new CacheOptions()));

        return services;
    }
}

