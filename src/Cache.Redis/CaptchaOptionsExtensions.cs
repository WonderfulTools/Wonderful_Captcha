namespace WonderfulCaptcha.Cache.Redis;
public static class CaptchaOptionsExtensions
{
    //public static void UseRedisCacheProvider(this IWonderfulCaptchaBuilder builder, IServiceCollection services)
    //    => services.TryAddScoped<ICacheProvider, RedisCacheProvider>();

    public static void UseRedisCacheProvider(this CaptchaOptions captchaOptions)
          => captchaOptions.CacheOptions.CacheProvider = typeof(RedisCacheProvider);

}
