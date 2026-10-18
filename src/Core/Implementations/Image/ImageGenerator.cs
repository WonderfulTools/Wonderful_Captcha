namespace WonderfulCaptcha.Images;

public class ImageGenerator : IImageGenerator
{
    private readonly ICanvasBuilder _canvasBuilder;
    private readonly IContentWriter _contentWriter;
    private readonly IFilterEngine _filterEngine;


    public ImageGenerator(ICanvasBuilder canvasBuilder, IContentWriter contentWriter, IFilterEngine filterEngine)
    {
        _canvasBuilder = canvasBuilder;
        _contentWriter = contentWriter;
        _filterEngine = filterEngine;
    }

    public Task<string> GenerateImageAsync(CaptchaOptions options, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var image = _canvasBuilder.Create(options);
        _contentWriter.WriteText(image, options);
        _filterEngine.ApplyFilters(image, options);
        return Task.FromResult(GeneratePng(image));
    }

    private static string GeneratePng(SKBitmap image)
    {
        using var data = image.Encode(SKEncodedImageFormat.Png, 100)
            ?? throw new InvalidOperationException("Failed to encode captcha image as PNG.");
        return $"data:image/png;base64,{Convert.ToBase64String(data.Span)}";
    }
}

