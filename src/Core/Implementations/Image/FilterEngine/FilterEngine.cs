namespace WonderfulCaptcha.Images;

public class FilterEngine : IFilterEngine
{
    // Random.Shared is thread-safe; a shared `new Random()` gets corrupted under concurrent use.
    private static readonly Random Random = Random.Shared;

    public void ApplyFilters(SKBitmap image, CaptchaOptions options)
    {
        AddLines(image, options);
        AddSaltAndPepper(image, options);
        AddOilPaint(image, options);
        AddDefaults(image);
    }

    /// <summary>Light gaussian blur (sigma 0.5) followed by 80% saturation.</summary>
    private static void AddDefaults(SKBitmap image)
    {
        using var source = SKImage.FromBitmap(image);   // copies the pixels of a mutable bitmap
        using var blur = SKImageFilter.CreateBlur(0.5f, 0.5f, SKShaderTileMode.Clamp);
        using var saturate = SKColorFilter.CreateColorMatrix(CreateSaturateMatrix(0.8f));
        using var paint = new SKPaint { ImageFilter = SKImageFilter.CreateColorFilter(saturate, blur) };
        using var canvas = new SKCanvas(image);

        canvas.Clear(SKColors.Transparent);
        canvas.DrawImage(source, 0, 0, paint);
    }

    // Standard luminance-preserving saturation matrix (W3C filter effects), 4x5 row-major for Skia.
    private static float[] CreateSaturateMatrix(float s) =>
    [
        0.213f + 0.787f * s, 0.715f - 0.715f * s, 0.072f - 0.072f * s, 0, 0,
        0.213f - 0.213f * s, 0.715f + 0.285f * s, 0.072f - 0.072f * s, 0, 0,
        0.213f - 0.213f * s, 0.715f - 0.715f * s, 0.072f + 0.928f * s, 0, 0,
        0,                   0,                   0,                   1, 0,
    ];

    private static void AddOilPaint(SKBitmap image, CaptchaOptions options)
    {
        if (options.NoiseOptions.OilPaintLevel > 0)
            OilPaint(image, options.NoiseOptions.OilPaintLevel, 8);
    }

    /// <summary>
    /// Oil-paint (mode) filter: each pixel takes the average colour of the most frequent intensity level
    /// in the window spanning <c>brushSize / 2</c> pixels up and to the left of it. Alpha is kept.
    /// </summary>
    private static void OilPaint(SKBitmap image, int levels, int brushSize)
    {
        levels = Math.Clamp(levels, 1, 255);
        int width = image.Width, height = image.Height;
        brushSize = Math.Clamp(brushSize, 1, Math.Min(width, height));
        var radius = brushSize >> 1;

        var source = image.Pixels;
        var target = new SKColor[source.Length];
        var counts = new int[levels];
        var sumR = new int[levels];
        var sumG = new int[levels];
        var sumB = new int[levels];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                Array.Clear(counts);
                Array.Clear(sumR);
                Array.Clear(sumG);
                Array.Clear(sumB);
                int maxIndex = 0, maxCount = 0;

                for (int fy = -radius; fy <= 0; fy++)
                {
                    var sy = Math.Clamp(y + fy, 0, height - 1);
                    for (int fx = -radius; fx <= 0; fx++)
                    {
                        var sample = source[sy * width + Math.Clamp(x + fx, 0, width - 1)];
                        var bin = (int)MathF.Round((sample.Red + sample.Green + sample.Blue) / (3f * 255f) * (levels - 1));
                        counts[bin]++;
                        sumR[bin] += sample.Red;
                        sumG[bin] += sample.Green;
                        sumB[bin] += sample.Blue;
                        if (counts[bin] > maxCount)
                        {
                            maxCount = counts[bin];
                            maxIndex = bin;
                        }
                    }
                }

                var original = source[y * width + x];
                target[y * width + x] = new SKColor(
                    (byte)(sumR[maxIndex] / maxCount),
                    (byte)(sumG[maxIndex] / maxCount),
                    (byte)(sumB[maxIndex] / maxCount),
                    original.Alpha);
            }

        image.Pixels = target;
    }

    private static void AddSaltAndPepper(SKBitmap image, CaptchaOptions options)
    {
        var density = options.NoiseOptions.SaltAndPepperDensityPercent / 100;
        if (density <= 0)
            return;

        for (int x = 0; x < image.Width; x++)
            for (int y = 0; y < image.Height; y++)
                if (Random.NextDouble() < density)
                    image.SetPixel(x, y, Random.Next(2) == 0 ? ColorUtils.GetColor(options.TextOptions.TextColor) : SKColors.White);
    }

    private static void AddLines(SKBitmap image, CaptchaOptions options)
    {
        var color = options.TextOptions.TextColorHex is not null ?
                    ColorUtils.GetColor(options.TextOptions.TextColorHex) :
                    ColorUtils.GetColor(options.TextOptions.TextColor);

        using var canvas = new SKCanvas(image);
        for (int i = 0; i < options.NoiseOptions.MaxLineNumbers; i++)
        {
            using var pen = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = Random.Next(1, 5),
            };
            var point = new SKPoint(Random.Next(0, options.ImageOptions.ImageSize.Width), Random.Next(options.ImageOptions.ImageSize.Height));
            var point2 = new SKPoint(Random.Next(0, options.ImageOptions.ImageSize.Width), Random.Next(options.ImageOptions.ImageSize.Height));
            canvas.DrawLine(point, point2, pen);
        }
    }
}