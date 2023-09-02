using Microsoft.Extensions.DependencyInjection;

namespace WonderfulCaptcha;
public static class CaptchaServiceCollectionExtensions
{
    public static void AddDNTCaptcha(this IServiceCollection services, Action<CaptchaOptions>? options = null)
    {
        //TODO add options to configs
    }
}