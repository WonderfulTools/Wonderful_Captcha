namespace WonderfulCaptcha.Image;

public interface IImageGenerator
{
    public Task<string> GenerateImageAsync(CancellationToken cancellationToken);
}

