namespace WonderfulCaptcha.Images;

public interface IImageGenerator
{
    public Task<string> GenerateImageAsync(CaptchaOptions options, CancellationToken cancellationToken = default!);
}

