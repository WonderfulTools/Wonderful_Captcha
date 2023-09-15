using Microsoft.Extensions.Options;

namespace WonderfulCaptcha.Image;

public class ImageGenerator : IImageGenerator
{
    private readonly CaptchaOptions captchaOptions;

    public ImageGenerator(IOptions<CaptchaOptions> option)
    {
        captchaOptions = option.Value;
    }

    public Task<string> GenerateImageAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

