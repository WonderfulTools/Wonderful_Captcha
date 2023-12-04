using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Drawing.Processing;

namespace WonderfulCaptcha.Images;

public class ContentWriter : IContentWriter
{
    private readonly CaptchaOptions _captchaOptions;

    public ContentWriter(IOptions<CaptchaOptions> captchaOptions)
        => _captchaOptions = captchaOptions.Value;


    public void WriteText(Image<Rgba32> image)
    {
        var text = _captchaOptions.TextOptions.Text;
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
        var font = FontUtils.GetFont(_captchaOptions.TextOptions.TextFont, fontSize, _captchaOptions.TextOptions.TextFontStyle);
        var color = _captchaOptions.TextOptions.TextColorHex is not null ?
            ColorUtils.GetColor(_captchaOptions.TextOptions.TextColorHex) :
            ColorUtils.GetColor(_captchaOptions.TextOptions.TextColor);
        var brush = BrushUtils.GetBrush(_captchaOptions.TextOptions.TextBrush, color);

        image.Mutate(x => x.DrawText(drawingOptions, c, font, brush, position));
    }

    private void PutShadowChar(Image<Rgba32> image, DrawingOptions drawingOptions, int fontSize, string c, PointF position)
    {
        if (!_captchaOptions.TextOptions.TextShadow)
            return;

        var font = FontUtils.GetFont(_captchaOptions.TextOptions.TextFont, fontSize, _captchaOptions.TextOptions.TextFontStyle);
        var brush = BrushUtils.GetBrush(_captchaOptions.TextOptions.TextBrush, Color.LightSlateGray);
        var shadowPosition = new PointF(position.X - 5, position.Y - 5);

        image.Mutate(x => x.DrawText(drawingOptions, c, font, brush, shadowPosition));
    }

    private int GetFontSize()
        => _captchaOptions.ImageOptions.ImageSizeStrategy switch
        {
            ImageSizeStrategy.Fit => SizeUtils.GetFitFontSize(_captchaOptions),
            ImageSizeStrategy.RelativeFit => SizeUtils.GetRelativeFitFontSize(_captchaOptions),
            _ => _captchaOptions.TextOptions.FontSize
        };

    private int RandomizeFontSize(int fontSize)
        => fontSize + Helpers.GetRandomNumberBetween(-_captchaOptions.TextOptions.FontSizeVarietyRange,
            _captchaOptions.TextOptions.FontSizeVarietyRange);

    private PointF GetCharPosition(int fontSize, int charIndex)
    {
        var charWidth = FontUtils.GetCharWidth(fontSize);

        var randomRangeX = _captchaOptions.ImageOptions.CharPositionVarietyRange.Width;
        var randomRangeY = _captchaOptions.ImageOptions.CharPositionVarietyRange.Height;

        var width = (charIndex * (charWidth + _captchaOptions.ImageOptions.CharSpacing)) +
                    _captchaOptions.ImageOptions.CharSpacing +
                    Helpers.GetRandomNumberBetween(-randomRangeX, randomRangeX);

        var height = Helpers.GetRandomNumberBetween(0, 2 * randomRangeY);

        return new PointF(width, height);
    }
}