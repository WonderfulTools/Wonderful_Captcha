using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

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

    public async Task<string> GenerateImageAsync(CancellationToken cancellationToken)
    {
            var image = _canvasBuilder.Create();
            _contentWriter.WriteText(image);
            _filterEngine.ApplyFilters(image);
            return await GeneratePng(image, cancellationToken);
    }

    private async Task<string> GeneratePng(Image<Rgba32> image, CancellationToken cancellationToken = default)
    {
        using MemoryStream memStream = new();
        await image.SaveAsPngAsync(memStream, cancellationToken);
        return $"data:image/png;base64,{Convert.ToBase64String(memStream.GetBuffer())}";
    }
}

