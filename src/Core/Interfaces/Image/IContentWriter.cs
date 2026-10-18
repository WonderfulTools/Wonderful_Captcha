namespace WonderfulCaptcha.Images;

public interface IContentWriter
{
    void WriteText(SKBitmap image, CaptchaOptions options);
}