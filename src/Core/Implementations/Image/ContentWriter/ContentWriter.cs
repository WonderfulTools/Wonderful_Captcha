using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WonderfulCaptcha.Images;

public class ContentWriter : IContentWriter
{
    public void WriteText(Image<Rgba32> image, CaptchaOptions options)
    {
        var text = options.TextOptions.Text;
        var fontSize = GetFontSize(options);

        for (int i = 0; i < text.Length; i++)
        {
            fontSize = RandomizeFontSize(fontSize, options);
            var character = text[i].ToString();
            var position = GetCharPosition(fontSize, i, options);

            DrawingOptions drawingOptions = new()
            {
                Transform = TextTransformFilter.ApplyTransforms(position, options)
            };

            PutShadowChar(image, drawingOptions, fontSize, character, position, options);
            PutChar(image, drawingOptions, fontSize, character, position, options);
        }
    }

    private static void PutChar(Image<Rgba32> image, DrawingOptions drawingOptions, int fontSize, string c, PointF position, CaptchaOptions options)
    {
        var font = FontUtils.GetFont(options.TextOptions.TextFont, fontSize, options.TextOptions.TextFontStyle);
        var color = options.TextOptions.TextColorHex is not null ?
            ColorUtils.GetColor(options.TextOptions.TextColorHex) :
            ColorUtils.GetColor(options.TextOptions.TextColor);
        var brush = BrushUtils.GetBrush(options.TextOptions.TextBrush, color);

        image.Mutate(x => x.DrawText(drawingOptions, c, font, brush, position));
    }

    private static void PutShadowChar(Image<Rgba32> image, DrawingOptions drawingOptions, int fontSize, string c, PointF position, CaptchaOptions options)
    {
        if (!options.TextOptions.TextShadow)
            return;

        var font = FontUtils.GetFont(options.TextOptions.TextFont, fontSize, options.TextOptions.TextFontStyle);
        var brush = BrushUtils.GetBrush(options.TextOptions.TextBrush, Color.LightSlateGray);
        var shadowPosition = new PointF(position.X - 5, position.Y - 5);

        image.Mutate(x => x.DrawText(drawingOptions, c, font, brush, shadowPosition));
    }

    private static int GetFontSize(CaptchaOptions options)
        => options.ImageOptions.ImageSizeStrategy switch
        {
            ImageSizeStrategy.Fit => SizeUtils.GetFitFontSize(options),
            ImageSizeStrategy.RelativeFit => SizeUtils.GetRelativeFitFontSize(options),
            _ => options.TextOptions.FontSize
        };

    private static int RandomizeFontSize(int fontSize, CaptchaOptions options)
        => fontSize + Helpers.GetRandomNumberBetween(-options.TextOptions.FontSizeVarietyRange,
            options.TextOptions.FontSizeVarietyRange);

    private static PointF GetCharPosition(int fontSize, int charIndex, CaptchaOptions options)
    {
        var charWidth = FontUtils.GetCharWidth(fontSize);

        var randomRangeX = options.ImageOptions.CharPositionVarietyRange.Width;
        var randomRangeY = options.ImageOptions.CharPositionVarietyRange.Height;

        var width = (charIndex * (charWidth + options.ImageOptions.CharSpacing)) +
                    options.ImageOptions.CharSpacing +
                    Helpers.GetRandomNumberBetween(-randomRangeX, randomRangeX);

        var height = Helpers.GetRandomNumberBetween(0, 2 * randomRangeY);

        return new PointF(width, height);
    }
}