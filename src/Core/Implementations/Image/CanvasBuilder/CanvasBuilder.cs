namespace WonderfulCaptcha.Images;

public class CanvasBuilder : ICanvasBuilder
{
    private readonly CaptchaOptions _captchaOptions;

    public CanvasBuilder(CaptchaOptions captchaOptions)
        => _captchaOptions = captchaOptions;
    

    public Image<Rgba32> Create()
    {
        var size = GetSize();
        Image<Rgba32> image = new Image<Rgba32>(size.Width, size.Height);
        return image;
    }

    private (int Width, int Height) GetSize()
        => _captchaOptions.SizeStrategy switch
        {
            SizeStrategy.Dynamic => SizeUtils.GetDynamicSize(_captchaOptions.FontSize, _captchaOptions),
            SizeStrategy.RelativeFit => SizeUtils.GetRelativeFitSize(_captchaOptions.FontSize, _captchaOptions),
            _ => _captchaOptions.Size
        };

}