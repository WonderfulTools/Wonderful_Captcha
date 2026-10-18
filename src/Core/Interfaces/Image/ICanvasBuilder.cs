namespace WonderfulCaptcha.Images;

/// <summary>
/// Creates the blank captcha bitmap (size + background) for one captcha.
/// </summary>
public interface ICanvasBuilder
{
    SKBitmap Create(CaptchaOptions options);
}