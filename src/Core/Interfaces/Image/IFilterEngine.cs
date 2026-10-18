namespace WonderfulCaptcha.Images;

public interface IFilterEngine
{
    void ApplyFilters(SKBitmap image, CaptchaOptions options);
}