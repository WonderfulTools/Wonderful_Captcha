namespace WonderfulCaptcha;

public class ImageOptions
{

    public (int Width, int Height) ImageSize { get; set; } = (350, 100);
    public ImageSizeStrategy ImageSizeStrategy { get; set; } = ImageSizeStrategy.Dynamic;
    public ColorEnum ImageBackgroundColor { get; set; } = ColorEnum.White;
    public string ImageBackgroundColorHex { get; set; } = default!;
    public int RelativeFitSizeThreshold { get; set; } = 25;
    public int CharSpacing { get; set; } = 10;
    public (int Width, int Height) CharPositionVarietyRange { get; set; } = (15, 15);
    public int Complexity { get; set; } = 0;
}
