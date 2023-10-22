namespace WonderfulCaptcha.Images;

public class CanvasBuilder : ICanvasBuilder
{
    private readonly CaptchaOptions _captchaOptions;

    public CanvasBuilder(CaptchaOptions captchaOptions)
        => _captchaOptions = captchaOptions;


    public Image<Rgba32> Create()
    {
        _captchaOptions.ImageSize = GetSize();
        var backgroundColor = _captchaOptions.ImageBackgroundColorHex is not null ? ColorUtils.GetColor(_captchaOptions.ImageBackgroundColorHex) : ColorUtils.GetColor(_captchaOptions.ImageBackgroundColor);
        Image<Rgba32> image = new(_captchaOptions.ImageSize.Width, _captchaOptions.ImageSize.Height, backgroundColor);
        return image;
    }

    private (int Width, int Height) GetSize()
        => _captchaOptions.ImageSizeStrategy switch
        {
            ImageSizeStrategy.Dynamic => SizeUtils.GetDynamicSize(_captchaOptions.FontSize, _captchaOptions),
            ImageSizeStrategy.RelativeFit => SizeUtils.GetRelativeFitSize(_captchaOptions.FontSize, _captchaOptions),
            _ => _captchaOptions.ImageSize
        };

}