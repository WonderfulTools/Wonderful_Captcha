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

    /// <summary>
    /// Deep copy used as a per-call working set. The registered options instance is a singleton
    /// shared by every request, so it must never be mutated while generating a captcha.
    /// </summary>
    public CaptchaOptions Clone() => new()
    {
        ImageOptions = ImageOptions.Clone(),
        TextOptions = TextOptions.Clone(),
        CacheOptions = CacheOptions.Clone(),
        NoiseOptions = NoiseOptions.Clone(),
    };
}
