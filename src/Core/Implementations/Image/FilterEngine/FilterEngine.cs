using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WonderfulCaptcha.Images;

public class FilterEngine : IFilterEngine
{
    private readonly CaptchaOptions _captchaOptions;
    private static readonly Random Random = new();

    public FilterEngine(IOptions<CaptchaOptions> captchaOptions)
        => _captchaOptions = captchaOptions.Value;


    public void ApplyFilters(Image<Rgba32> image)
    {
        AddLines(image);
        AddSaltAndPepper(image);
        AddOilPaint(image);
        AddDefaults(image);
    }

    private static void AddDefaults(Image<Rgba32> image) =>
        image.Mutate(x => x.GaussianBlur(0.5f).Saturate(0.8f));


    private void AddOilPaint(Image<Rgba32> image)
    {
        if (_captchaOptions.NoiseOptions.OilPaintLevel > 0)
            image.Mutate(x => x.OilPaint(_captchaOptions.NoiseOptions.OilPaintLevel, 8));
    }

    private void AddSaltAndPepper(Image<Rgba32> image)
    {
        var random = new Random();
        for (int x = 0; x < image.Width; x++)
            for (int y = 0; y < image.Height; y++)
                if (random.NextDouble() < _captchaOptions.NoiseOptions.SaltAndPepperDensityPercent / 100)
                    image[x, y] = random.Next(2) == 0 ? ColorUtils.GetColor(_captchaOptions.TextOptions.TextColor) : Color.White;
    }

    private void AddLines(Image<Rgba32> image)
    {
        var color = _captchaOptions.TextOptions.TextColorHex is not null ?
                    ColorUtils.GetColor(_captchaOptions.TextOptions.TextColorHex) :
                    ColorUtils.GetColor(_captchaOptions.TextOptions.TextColor);
        for (int i = 0; i < _captchaOptions.NoiseOptions.MaxLineNumbers; i++)
        {
            var brush = BrushUtils.GetBrush(BrushEnum.Solid, color);
            var pen = Pens.Solid(brush, Random.Next(1, 5));
            var point = new PointF(Random.Next(0, _captchaOptions.ImageOptions.ImageSize.Width), Random.Next(_captchaOptions.ImageOptions.ImageSize.Height));
            var point2 = new PointF(Random.Next(0, _captchaOptions.ImageOptions.ImageSize.Width), Random.Next(_captchaOptions.ImageOptions.ImageSize.Height));
            image.Mutate(s => s.DrawLine(pen, new PointF[] { point, point2 }));
        }
    }
}