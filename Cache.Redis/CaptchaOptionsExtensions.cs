using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WonderfulCaptcha.Cache.Redis;

namespace WonderfulCaptcha.Cache.InMemory;
public static class CaptchaOptionsExtensions
{
    public static CaptchaOptions UseRedisCacheProvider(this CaptchaOptions captchaOptions, IServiceCollection services, TimeSpan? cacheExpirationTime = default!)
    {
        services.TryAddScoped<ICacheProvider, RedisCacheProvider>();
        return captchaOptions;
    }
}
