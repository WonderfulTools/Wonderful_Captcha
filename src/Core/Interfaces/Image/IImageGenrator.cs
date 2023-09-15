namespace WonderfulCaptcha.Images;

public interface IImageGenerator
{
    public Task<string> GenerateImageAsync(CancellationToken cancellationToken);
}

