using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace WonderfulCaptcha.Images;

public class ImageGenerator : IImageGenerator
{
    private readonly CaptchaOptions captchaOptions;
    private static Random random = new Random();

    public ImageGenerator(CaptchaOptions option)
    {
        captchaOptions = option;
    }

    public async Task<string> GenerateImageAsync(CancellationToken cancellationToken)
    {
        try
        {
            Font font = new Font(SystemFonts.Get("Arial"), captchaOptions.FontSize, FontStyle.Italic);
            var size = FixImageSize(font);

            Image<Rgba32> image = new Image<Rgba32>(captchaOptions.Size.Width, captchaOptions.Size.Height);


            for (int i = 0; i < captchaOptions.Text.Length; i++)
            {
                var startPointX = i > 0 ? i * size - random.Next(5, 20) : 5;
                var startPointY = 5 + random.Next(-15, 15);
                var brush = i == captchaOptions.Text.Length - 1 ? Brushes.Solid(GetRandomColor()) : GetRandomBrush();
                font = new Font(SystemFonts.Get("Arial"), captchaOptions.FontSize - random.Next(50), GetRandomFontStyle());
                image.Mutate(x => x.DrawText(captchaOptions.Text[i].ToString(), font, Color.LightSlateGray, new PointF(startPointX - 5, startPointY - 5)));
                image.Mutate(x => x.DrawText(captchaOptions.Text[i].ToString(), font, brush, new PointF(startPointX, startPointY)));
            }

            AddNoise(image);

            using MemoryStream memStream = new();
            await image.SaveAsPngAsync(memStream, cancellationToken);
            return $"data:image/png;base64,{Convert.ToBase64String(memStream.GetBuffer())}";
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private float FixImageSize(Font font)
    {
        var textSize = TextMeasurer.MeasureSize("A", new TextOptions(font));
        if (textSize.Width * (captchaOptions.Text.Length - 1) > captchaOptions.Size.Width)
            captchaOptions.Size = (Convert.ToInt32(textSize.Width * (captchaOptions.Text.Length - 1)) + 10, captchaOptions.Size.Height);
        if (textSize.Height > captchaOptions.Size.Height)
            captchaOptions.Size = (captchaOptions.Size.Width, Convert.ToInt32(textSize.Height) + 10);
        return textSize.Width;
    }

    private void AddNoise(Image<Rgba32> image)
    {
        image.Mutate(x => x.GaussianBlur(0.5f)
                               .Saturate(0.8f));

        var random = new Random();
        for (int x = 0; x < image.Width; x++)
            for (int y = 0; y < image.Height; y++)
                if (random.NextDouble() < captchaOptions.Nosie.Density)
                    image[x, y] = random.Next(2) == 0 ? GetColor(captchaOptions.Color) : Color.White;

    }

    private Color GetColor(ColorEnum color) =>
        color switch
        {
            ColorEnum.Red => Color.Red,
            ColorEnum.Green => Color.Green,
            ColorEnum.Yellow => Color.Yellow,
            ColorEnum.Black => Color.Black,
            ColorEnum.Gray => Color.Gray,
            ColorEnum.Blue => Color.Blue,
            _ => Color.Green,
        };
    private Color GetRandomColor()
        => GetColor((ColorEnum)random.Next(10));
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

