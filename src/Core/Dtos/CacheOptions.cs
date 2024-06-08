namespace WonderfulCaptcha;

public class CacheOptions
{
    public Type CacheProvider { get; set; } = default!;
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(1);
    public string Prefix { get; set; } = "WonderfulCaptcha";

}