namespace WonderfulCaptcha.Images;

public class ContentWriter : IContentWriter
{
    public void WriteText(SKBitmap image, CaptchaOptions options)
    {
        var text = options.TextOptions.Text;
        var fontSize = GetFontSize(options);

        using var canvas = new SKCanvas(image);
        for (int i = 0; i < text.Length; i++)
        {
            fontSize = RandomizeFontSize(fontSize, options);
            var character = text[i].ToString();
            var position = GetCharPosition(fontSize, i, options);

            canvas.Save();
            canvas.SetMatrix(TextTransformFilter.ApplyTransforms(position, options));

            PutShadowChar(canvas, fontSize, character, position, options);
            PutChar(canvas, fontSize, character, position, options);

            canvas.Restore();
        }
    }

    private static void PutChar(SKCanvas canvas, int fontSize, string c, SKPoint position, CaptchaOptions options)
    {
        using var font = FontUtils.GetFont(options.TextOptions.TextFont, fontSize, options.TextOptions.TextFontStyle);
        var color = options.TextOptions.TextColorHex is not null ?
            ColorUtils.GetColor(options.TextOptions.TextColorHex) :
            ColorUtils.GetColor(options.TextOptions.TextColor);
        using var brush = BrushUtils.GetBrush(options.TextOptions.TextBrush, color, canvas.TotalMatrix);

        DrawText(canvas, c, position, font, brush);
    }

    private static void PutShadowChar(SKCanvas canvas, int fontSize, string c, SKPoint position, CaptchaOptions options)
    {
        if (!options.TextOptions.TextShadow)
            return;

        using var font = FontUtils.GetFont(options.TextOptions.TextFont, fontSize, options.TextOptions.TextFontStyle);
        using var brush = BrushUtils.GetBrush(options.TextOptions.TextBrush, SKColors.LightSlateGray, canvas.TotalMatrix);
        var shadowPosition = new SKPoint(position.X - 5, position.Y - 5);

        DrawText(canvas, c, shadowPosition, font, brush);
    }

    // position is the top-left of the text box; Skia draws at the baseline.
    private static void DrawText(SKCanvas canvas, string c, SKPoint position, SKFont font, SKPaint brush)
        => canvas.DrawText(c, position.X, position.Y + FontUtils.GetBaselineOffset(font), font, brush);

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

    private static SKPoint GetCharPosition(int fontSize, int charIndex, CaptchaOptions options)
    {
        var charWidth = FontUtils.GetCharWidth(fontSize);

        var randomRangeX = options.ImageOptions.CharPositionVarietyRange.Width;
        var randomRangeY = options.ImageOptions.CharPositionVarietyRange.Height;

        var width = (charIndex * (charWidth + options.ImageOptions.CharSpacing)) +
                    options.ImageOptions.CharSpacing +
                    Helpers.GetRandomNumberBetween(-randomRangeX, randomRangeX);

        var height = Helpers.GetRandomNumberBetween(0, 2 * randomRangeY);

        return new SKPoint(width, height);
    }
}