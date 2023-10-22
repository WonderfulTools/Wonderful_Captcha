using SixLabors.ImageSharp.Drawing.Processing;

namespace WonderfulCaptcha.Images;

public class FilterEngine : IFilterEngine
{
    private readonly CaptchaOptions _captchaOptions;
    private static readonly Random Random = new();

    public FilterEngine(CaptchaOptions captchaOptions)
        => _captchaOptions = captchaOptions;


    public void ApplyFilters(Image<Rgba32> image)
    {
        var color = _captchaOptions.TextColorHex is not null ? ColorUtils.GetColor(_captchaOptions.TextColorHex) : ColorUtils.GetColor(_captchaOptions.TextColor);
        var brush = BrushUtils.GetBrush(_captchaOptions.TextBrush, color);
        for (int i = 0; i < _captchaOptions.Noise.MaxLineNumbers; i++)
        {
            var pen = Pens.Solid(brush, Random.Next(1, 5));
            var point = new PointF(Random.Next(0, _captchaOptions.ImageSize.Width), Random.Next(_captchaOptions.ImageSize.Height));
            var point2 = new PointF(Random.Next(0, _captchaOptions.ImageSize.Width), Random.Next(_captchaOptions.ImageSize.Height));
            image.Mutate(s => s.DrawLine(pen, new PointF[] { point, point2 }));
        }

        var random = new Random();
        for (int x = 0; x < image.Width; x++)
            for (int y = 0; y < image.Height; y++)
                if (random.NextDouble() < _captchaOptions.Noise.SaltAndPepperDensity)
                    image[x, y] = random.Next(2) == 0 ? ColorUtils.GetColor(_captchaOptions.TextColor) : Color.White;

        image.Mutate(x => x
            .GaussianBlur(0.5f)
            .Saturate(0.8f)
            .OilPaint(_captchaOptions.Noise.OilPaintLevel, 8)
            );

    }
}