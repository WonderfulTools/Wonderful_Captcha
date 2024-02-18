namespace WonderfulCaptcha.Cache.EasyCaching;
public static class CaptchaOptionsExtensions
{
    //public static void UseInMemoryCacheProvider(this IWonderfulCaptchaBuilder builder, IServiceCollection services)
    //    => services.TryAddScoped<ICacheProvider, EasyCachingProvider>();

    public static void UseEasyCachingProvider(this CaptchaOptions captchaOptions)
          => captchaOptions.CacheOptions.CacheProvider = typeof(EasyCachingProvider);
}
