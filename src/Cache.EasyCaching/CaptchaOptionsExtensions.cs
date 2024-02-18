using WonderfulCaptcha.Cache.EasyCaching;

namespace WonderfulCaptcha.Cache.InMemory;
public static class CaptchaOptionsExtensions
{
    //public static void UseInMemoryCacheProvider(this IWonderfulCaptchaBuilder builder, IServiceCollection services)
    //    => services.TryAddScoped<ICacheProvider, EasyCachingProvider>();

    public static void UseInMemoryCacheProvider(this CaptchaOptions captchaOptions)
          => captchaOptions.CacheOptions.CacheProvider = typeof(EasyCachingProvider);
}
