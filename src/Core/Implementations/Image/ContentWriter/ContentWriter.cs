using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace WonderfulCaptcha.Images;

public class ContentWriter : IContentWriter
{
    private readonly CaptchaOptions _captchaOptions;
    private static Random random = new Random();

    public ContentWriter(CaptchaOptions captchaOptions)
        => _captchaOptions = captchaOptions;
    

    public void WriteText(Image<Rgba32> image)
    {
        Font font = new Font(SystemFonts.Get("Arial"), _captchaOptions.FontSize, FontStyle.Italic);
        var size = FixImageSize(font);
            
        for (int i = 0; i < _captchaOptions.Text.Length; i++)
        {
            var startPointX = i > 0 ? i * size - random.Next(5, 20) : 5;
            var startPointY = 5 + random.Next(-15, 15);
            var brush = i == _captchaOptions.Text.Length - 1 ? Brushes.Solid(GetRandomColor()) : GetRandomBrush();
            font = new Font(SystemFonts.Get("Arial"), _captchaOptions.FontSize - random.Next(50), GetRandomFontStyle());
            image.Mutate(x => x.DrawText(_captchaOptions.Text[i].ToString(), font, Color.LightSlateGray, new PointF(startPointX - 5, startPointY - 5)));
            image.Mutate(x => x.DrawText(_captchaOptions.Text[i].ToString(), font, brush, new PointF(startPointX, startPointY)));
        }
    }
    
    private float FixImageSize(Font font)
    {
        var textSize = TextMeasurer.MeasureSize("A", new TextOptions(font));
        if (textSize.Width * (_captchaOptions.Text.Length - 1) > _captchaOptions.Size.Width)
            _captchaOptions.Size = (Convert.ToInt32(textSize.Width * (_captchaOptions.Text.Length - 1)) + 10, _captchaOptions.Size.Height);
        if (textSize.Height > _captchaOptions.Size.Height)
            _captchaOptions.Size = (_captchaOptions.Size.Width, Convert.ToInt32(textSize.Height) + 10);
        return textSize.Width;
    }
    
    
    private Color GetRandomColor()
        => ColorUtils.GetColor((ColorEnum)random.Next(10));
    private Brush GetRandomBrush()
    {
        var color = GetRandomColor();
        return random.Next(15) switch
        {
            1 => Brushes.Percent20(color),
            2 => Brushes.BackwardDiagonal(color),
            3 => Brushes.ForwardDiagonal(color),
            4 => Brushes.Horizontal(color),
            5 => Brushes.Min(color),
            6 => Brushes.Vertical(color),
            _ => Brushes.Solid(color),
        };
    }
    private FontStyle GetRandomFontStyle()
    {
        var color = GetRandomColor();
        return random.Next(5) switch
        {
            1 => FontStyle.Italic,
            2 => FontStyle.Bold,
            3 => FontStyle.BoldItalic,
            _ => FontStyle.Regular,
        };
    }
}