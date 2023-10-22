using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WonderfulCaptcha.Cache.EasyCaching;

namespace WonderfulCaptcha.Cache.InMemory;
public static class CaptchaOptionsExtensions
{
    public static CaptchaOptions UseInMemoryCacheProvider(this CaptchaOptions captchaOptions, IServiceCollection services, TimeSpan? cacheExpirationTime = default!)
    {
        services.TryAddScoped<ICacheProvider, EasyCachingProvider>();
        return captchaOptions;
    }
}
