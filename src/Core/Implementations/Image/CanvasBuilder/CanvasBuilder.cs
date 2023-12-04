using Microsoft.Extensions.Options;

namespace WonderfulCaptcha.Images;

public class CanvasBuilder : ICanvasBuilder
{
    private readonly CaptchaOptions _captchaOptions;

    public CanvasBuilder(IOptions<CaptchaOptions> captchaOptions)
        => _captchaOptions = captchaOptions.Value;


    public Image<Rgba32> Create()
    {
        _captchaOptions.ImageOptions.ImageSize = GetSize();
        var backgroundColor = _captchaOptions.ImageOptions.ImageBackgroundColorHex is not null ? ColorUtils.GetColor(_captchaOptions.ImageOptions.ImageBackgroundColorHex) : ColorUtils.GetColor(_captchaOptions.ImageOptions.ImageBackgroundColor);
        Image<Rgba32> image = new(_captchaOptions.ImageOptions.ImageSize.Width, _captchaOptions.ImageOptions.ImageSize.Height, backgroundColor);
        return image;
    }

    private (int Width, int Height) GetSize()
        => _captchaOptions.ImageOptions.ImageSizeStrategy switch
        {
            ImageSizeStrategy.Dynamic => SizeUtils.GetDynamicSize(_captchaOptions.TextOptions.FontSize, _captchaOptions),
            ImageSizeStrategy.RelativeFit => SizeUtils.GetRelativeFitSize(_captchaOptions.TextOptions.FontSize, _captchaOptions),
            _ => _captchaOptions.ImageOptions.ImageSize
        };

}