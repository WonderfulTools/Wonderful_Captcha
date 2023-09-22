namespace WonderfulCaptcha.Images;

public static class ImageUtils
{
    public static Color GetColor(ColorEnum color) =>
        color switch
        {
            ColorEnum.Red => Color.Red,
            ColorEnum.Green => Color.Green,
            ColorEnum.Yellow => Color.Yellow,
            ColorEnum.Black => Color.Black,
            ColorEnum.Gray => Color.Gray,
            ColorEnum.Blue => Color.Blue,
            _ => Color.Green,
        };
}