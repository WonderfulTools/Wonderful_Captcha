using WonderfulCaptcha.Cache;

namespace WonderfulCaptcha;

public class CaptchaOptions
{
    public ImageOptions ImageOptions { get; set; } = new ImageOptions();
    public TextOptions TextOptions { get; set; } = new TextOptions();
    public CacheOptions CacheOptions { get; set; } = new CacheOptions();
    public NoiseOptions NoiseOptions { get; set; } = new NoiseOptions();

    public CaptchaOptions UseCustomCacheProvider<T>() where T : class, ICacheProvider
    {
        CacheOptions.CacheProvider = typeof(T);
        return this;
    }
}
