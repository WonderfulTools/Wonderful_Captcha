using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;
public static class CaptchaServiceCollectionExtensions
{
    public static IWonderfulCaptchaBuilder AddWonderfulCaptcha(this IServiceCollection services, IConfiguration configuration, Action<CaptchaOptions>? options = null)
    {
        services.AddScoped<IWonderfulCaptchaService, WonderfulCaptchaService>();
        services.AddScoped<IContentWriter, ContentWriter>();
        services.AddScoped<IFilterEngine, FilterEngine>();
        services.AddScoped<ICanvasBuilder, CanvasBuilder>();
        services.AddScoped<ICryptoProvider, SHA256CryptoEngine>();
        services.AddScoped<ITextProvider, TextFactory>();
        services.AddScoped<IImageGenerator, ImageGenerator>();
        services.Configure(options);
        return new WonderfulCaptchaBuilder();
    }
}

public interface IWonderfulCaptchaBuilder
{

}
public class WonderfulCaptchaBuilder : IWonderfulCaptchaBuilder
{

}