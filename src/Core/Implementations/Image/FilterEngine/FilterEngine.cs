using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WonderfulCaptcha.Images;

public class FilterEngine : IFilterEngine
{
    // Random.Shared is thread-safe; a shared `new Random()` gets corrupted under concurrent use.
    private static readonly Random Random = Random.Shared;

    public void ApplyFilters(Image<Rgba32> image, CaptchaOptions options)
    {
        AddLines(image, options);
        AddSaltAndPepper(image, options);
        AddOilPaint(image, options);
        AddDefaults(image);
    }

    private static void AddDefaults(Image<Rgba32> image) =>
        image.Mutate(x => x.GaussianBlur(0.5f).Saturate(0.8f));


    private static void AddOilPaint(Image<Rgba32> image, CaptchaOptions options)
    {
        if (options.NoiseOptions.OilPaintLevel > 0)
            image.Mutate(x => x.OilPaint(options.NoiseOptions.OilPaintLevel, 8));
    }

    private static void AddSaltAndPepper(Image<Rgba32> image, CaptchaOptions options)
    {
        for (int x = 0; x < image.Width; x++)
            for (int y = 0; y < image.Height; y++)
                if (Random.NextDouble() < options.NoiseOptions.SaltAndPepperDensityPercent / 100)
                    image[x, y] = Random.Next(2) == 0 ? ColorUtils.GetColor(options.TextOptions.TextColor) : Color.White;
    }

    private static void AddLines(Image<Rgba32> image, CaptchaOptions options)
    {
        var color = options.TextOptions.TextColorHex is not null ?
                    ColorUtils.GetColor(options.TextOptions.TextColorHex) :
                    ColorUtils.GetColor(options.TextOptions.TextColor);
        for (int i = 0; i < options.NoiseOptions.MaxLineNumbers; i++)
        {
            var brush = BrushUtils.GetBrush(BrushEnum.Solid, color);
            var pen = Pens.Solid(brush, Random.Next(1, 5));
            var point = new PointF(Random.Next(0, options.ImageOptions.ImageSize.Width), Random.Next(options.ImageOptions.ImageSize.Height));
            var point2 = new PointF(Random.Next(0, options.ImageOptions.ImageSize.Width), Random.Next(options.ImageOptions.ImageSize.Height));
            image.Mutate(s => s.DrawLine(pen, new PointF[] { point, point2 }));
        }
    }
}