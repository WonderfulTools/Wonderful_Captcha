namespace WonderfulCaptcha.Images;

public class CanvasBuilder : ICanvasBuilder
{
    private readonly CaptchaOptions _captchaOptions;

    public CanvasBuilder(CaptchaOptions captchaOptions)
        => _captchaOptions = captchaOptions;
    

    public Image<Rgba32> Create()
    {
        Image<Rgba32> image = new Image<Rgba32>(_captchaOptions.Size.Width, _captchaOptions.Size.Height);
        return image;


    }
}