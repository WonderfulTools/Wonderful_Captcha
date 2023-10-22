using SixLabors.ImageSharp.Drawing.Processing;

namespace WonderfulCaptcha.Images;

public class ContentWriter : IContentWriter
{
    private readonly CaptchaOptions _captchaOptions;

    public ContentWriter(CaptchaOptions captchaOptions)
        => _captchaOptions = captchaOptions;


    public void WriteText(Image<Rgba32> image)
    {
        var text = _captchaOptions.Text;
        var fontSize = GetFontSize();

        for (int i = 0; i < text.Length; i++)
        {
            fontSize = RandomizeFontSize(fontSize);
            var character = text[i].ToString();
            var position = GetCharPosition(fontSize, i);

            DrawingOptions drawingOptions = new()
            {
                Transform = TextTransformFilter.ApplyTransforms(position, _captchaOptions)
            };

            PutShadowChar(image, drawingOptions, fontSize, character, position);
            PutChar(image, drawingOptions, fontSize, character, position);
        }
    }

    private void PutChar(Image<Rgba32> image, DrawingOptions drawingOptions, int fontSize, string c, PointF position)
    {
        var font = FontUtils.GetFont(_captchaOptions.TextFont, fontSize, _captchaOptions.TextFontStyle);
        var color = _captchaOptions.TextColorHex is not null ? ColorUtils.GetColor(_captchaOptions.TextColorHex) : ColorUtils.GetColor(_captchaOptions.TextColor);
        var brush = BrushUtils.GetBrush(_captchaOptions.TextBrush, color);

        image.Mutate(x => x.DrawText(drawingOptions, c, font, brush, position));
    }
    private void PutShadowChar(Image<Rgba32> image, DrawingOptions drawingOptions, int fontSize, string c, PointF position)
    {
        if (!_captchaOptions.TextShadow)
            return;

        var font = FontUtils.GetFont(_captchaOptions.TextFont, fontSize, _captchaOptions.TextFontStyle);
        var brush = BrushUtils.GetBrush(_captchaOptions.TextBrush, Color.LightSlateGray);
        var shadowPosition = new PointF(position.X - 5, position.Y - 5);

        image.Mutate(x => x.DrawText(drawingOptions, c, font, brush, shadowPosition));
    }

    private int GetFontSize()
        => _captchaOptions.ImageSizeStrategy switch
        {
            ImageSizeStrategy.Fit => SizeUtils.GetFitFontSize(_captchaOptions),
            ImageSizeStrategy.RelativeFit => SizeUtils.GetRelativeFitFontSize(_captchaOptions),
            _ => _captchaOptions.FontSize
        };

    private int RandomizeFontSize(int fontSize)
        => fontSize + Helpers.GetRandomNumberBetween(-_captchaOptions.FontSizeVarietyRange,
            _captchaOptions.FontSizeVarietyRange);

    private PointF GetCharPosition(int fontSize, int charIndex)
    {
        var charWidth = FontUtils.GetCharWidth(fontSize);

        var randomRangeX = _captchaOptions.CharPositionVarietyRange.Width;
        var randomRangeY = _captchaOptions.CharPositionVarietyRange.Height;

        var width = (charIndex * (charWidth + _captchaOptions.CharSpacing)) +
                    _captchaOptions.CharSpacing +
                    Helpers.GetRandomNumberBetween(-randomRangeX, randomRangeX);

        var height = Helpers.GetRandomNumberBetween(0, 2 * randomRangeY);

        return new PointF(width, height);
    }
}