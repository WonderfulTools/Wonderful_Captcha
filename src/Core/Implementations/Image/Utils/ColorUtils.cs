namespace WonderfulCaptcha.Images;

public static class ColorUtils
{
    private static readonly Random Random = new();
    public static Color GetColor(ColorEnum color) =>
        color switch
        {
            ColorEnum.Red => Color.Red,
            ColorEnum.Green => Color.Green,
            ColorEnum.Black => Color.Black,
            ColorEnum.Gray => Color.Gray,
            ColorEnum.Blue => Color.Blue,
            ColorEnum.White => Color.White,
            _ => GetColor((ColorEnum)Random.Next(1, 7)),
        };
    public static Color GetColor(string hex) => Color.ParseHex(hex);
    public static Color GetColor(byte r, byte g, byte b) => Color.FromRgb(r, g, b);
    public static Color GetColor(byte r, byte g, byte b, byte a) => Color.FromRgba(r, g, b, a);
}