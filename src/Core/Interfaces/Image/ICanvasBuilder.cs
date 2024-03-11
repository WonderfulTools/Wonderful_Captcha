using SixLabors.ImageSharp.PixelFormats;

namespace WonderfulCaptcha.Images;

/// <summary>
/// 
/// </summary>
public interface ICanvasBuilder
{
    Image<Rgba32> Create();
}