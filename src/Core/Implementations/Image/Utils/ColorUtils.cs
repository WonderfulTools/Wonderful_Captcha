namespace WonderfulCaptcha.Images;

public static class ColorUtils
{
    private static readonly Random Random = new Random();
    public static Color GetColor(ColorEnum color) =>
        color switch
        {
            ColorEnum.Red => Color.Red,
            ColorEnum.Green => Color.Green,
            ColorEnum.Black => Color.Black,
            ColorEnum.Gray => Color.Gray,
            ColorEnum.Blue => Color.Blue,
            _ => GetColor((ColorEnum)Random.Next(6)),
        };
}