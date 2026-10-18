using SixLabors.ImageSharp.PixelFormats;

namespace WonderfulCaptcha.Images;

public class CanvasBuilder : ICanvasBuilder
{
    public Image<Rgba32> Create(CaptchaOptions options)
    {
        options.ImageOptions.ImageSize = GetSize(options);
        var backgroundColor = options.ImageOptions.ImageBackgroundColorHex is not null ?
                                ColorUtils.GetColor(options.ImageOptions.ImageBackgroundColorHex) :
                                ColorUtils.GetColor(options.ImageOptions.ImageBackgroundColor);
        Image<Rgba32> image = new(options.ImageOptions.ImageSize.Width, options.ImageOptions.ImageSize.Height, backgroundColor);
        return image;
    }

    private static (int Width, int Height) GetSize(CaptchaOptions options)
        => options.ImageOptions.ImageSizeStrategy switch
        {
            ImageSizeStrategy.Dynamic => SizeUtils.GetDynamicSize(options.TextOptions.FontSize, options),
            ImageSizeStrategy.RelativeFit => SizeUtils.GetRelativeFitSize(options.TextOptions.FontSize, options),
            _ => options.ImageOptions.ImageSize
        };

}