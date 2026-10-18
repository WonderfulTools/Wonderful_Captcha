namespace WonderfulCaptcha.Images;

public class CanvasBuilder : ICanvasBuilder
{
    public SKBitmap Create(CaptchaOptions options)
    {
        options.ImageOptions.ImageSize = GetSize(options);
        var backgroundColor = options.ImageOptions.ImageBackgroundColorHex is not null ?
                                ColorUtils.GetColor(options.ImageOptions.ImageBackgroundColorHex) :
                                ColorUtils.GetColor(options.ImageOptions.ImageBackgroundColor);

        var image = new SKBitmap(new SKImageInfo(options.ImageOptions.ImageSize.Width, options.ImageOptions.ImageSize.Height,
                                                 SKColorType.Rgba8888, SKAlphaType.Premul));
        image.Erase(backgroundColor);
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