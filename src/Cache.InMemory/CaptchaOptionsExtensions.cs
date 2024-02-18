namespace WonderfulCaptcha.Cache.InMemory;
public static class CaptchaOptionsExtensions
{
    public static void UseInMemoryCacheProvider(this CaptchaOptions captchaOptions)
          => captchaOptions.CacheOptions.CacheProvider = typeof(InMemoryCacheProvider);
}
