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
        Image image = new Image<Rgba32>(captchaOptions.Size.Width, captchaOptions.Size.Height);
        Font font = new Font(SystemFonts.Get("Arial"), 24, FontStyle.Italic);
        image.Mutate(x => x.DrawText(captchaOptions.Text, font, Color.Green, new PointF(0, 0)));
        using MemoryStream memStream = new();
        await image.SaveAsPngAsync(memStream, cancellationToken);
        return $"data:image/png;base64,{Convert.ToBase64String(memStream.GetBuffer())}";
    }
}

