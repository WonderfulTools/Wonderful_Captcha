using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WonderfulCaptcha.Cache.Redis;

namespace WonderfulCaptcha.Cache.InMemory;
public static class CaptchaOptionsExtensions
{
    public static IWonderfulCaptchaBuilder UseRedisCacheProvider(this IWonderfulCaptchaBuilder builder, IServiceCollection services)
    {
        services.TryAddScoped<ICacheProvider, RedisCacheProvider>();
        return builder;
    }
}
