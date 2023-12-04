namespace WonderfulCaptcha;

public class CacheOptions
{
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(1);

}