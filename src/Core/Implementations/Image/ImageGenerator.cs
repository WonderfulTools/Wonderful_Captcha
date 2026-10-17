using SixLabors.ImageSharp.PixelFormats;

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

    public async Task<string> GenerateImageAsync(CaptchaOptions options, CancellationToken cancellationToken)
    {
        using var image = _canvasBuilder.Create(options);
        _contentWriter.WriteText(image, options);
        _filterEngine.ApplyFilters(image, options);
        return await GeneratePng(image, cancellationToken);
    }

    private async Task<string> GeneratePng(Image<Rgba32> image, CancellationToken cancellationToken = default)
    {
        using MemoryStream memStream = new();
        await image.SaveAsPngAsync(memStream, cancellationToken);
        // ToArray (not GetBuffer) so the unused tail of the stream buffer is not appended to the PNG.
        return $"data:image/png;base64,{Convert.ToBase64String(memStream.ToArray())}";
    }
}

