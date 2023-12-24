namespace WonderfulCaptcha;

public class CaptchaOptions
{
    public ImageOptions ImageOptions { get; set; } = new();
    public TextOptions TextOptions { get; set; } = new();
    public CacheOptions CacheOptions { get; set; } = new();
    public NoiseOptions NoiseOptions { get; set; } = new();
}
