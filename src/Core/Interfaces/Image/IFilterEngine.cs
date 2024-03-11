using SixLabors.ImageSharp.PixelFormats;

namespace WonderfulCaptcha.Images;

public interface IFilterEngine
{
    void ApplyFilters(Image<Rgba32> image);
}