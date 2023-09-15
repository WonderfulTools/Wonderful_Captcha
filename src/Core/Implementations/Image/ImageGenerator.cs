using Microsoft.Extensions.Options;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace WonderfulCaptcha.Images;

public class ImageGenerator : IImageGenerator
{
    private readonly CaptchaOptions captchaOptions;

    public ImageGenerator(IOptions<CaptchaOptions> option)
    {
        captchaOptions = option.Value;
    }

    public async Task<string> GenerateImageAsync(CancellationToken cancellationToken)
    {
        Font font = new Font(SystemFonts.Get("Arial"), captchaOptions.FontSize, FontStyle.Italic);
        FixImageSize(font);

        Image<Rgba32> image = new Image<Rgba32>(captchaOptions.Size.Width, captchaOptions.Size.Height);
        image.Mutate(x => x.DrawText(captchaOptions.Text, font, GetColor(captchaOptions.Color), new PointF(5, 5)));
        AddNoise(image);

        using MemoryStream memStream = new();
        await image.SaveAsPngAsync(memStream, cancellationToken);
        return $"data:image/png;base64,{Convert.ToBase64String(memStream.GetBuffer())}";
    }

    private void FixImageSize(Font font)
    {
        var textSize = TextMeasurer.MeasureSize(captchaOptions.Text, new TextOptions(font));
        if (textSize.Width > captchaOptions.Size.Width)
            captchaOptions.Size = (Convert.ToInt32(textSize.Width) + 10, captchaOptions.Size.Height);
        if (textSize.Height > captchaOptions.Size.Height)
            captchaOptions.Size = (captchaOptions.Size.Width, Convert.ToInt32(textSize.Height) + 10);
    }

    private void AddNoise(Image<Rgba32> image)
    {
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
            _ => Color.Green,
        };
}

