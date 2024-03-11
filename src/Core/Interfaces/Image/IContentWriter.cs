using SixLabors.ImageSharp.PixelFormats;

namespace WonderfulCaptcha.Images;

public interface IContentWriter
{
    void WriteText(Image<Rgba32> image);
}