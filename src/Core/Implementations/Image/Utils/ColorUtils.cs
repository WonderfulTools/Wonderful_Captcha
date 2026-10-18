using System.Globalization;

namespace WonderfulCaptcha.Images;

public static class ColorUtils
{
    private static readonly Random Random = Random.Shared;
    public static SKColor GetColor(ColorEnum color) =>
        color switch
        {
            ColorEnum.Red => SKColors.Red,
            ColorEnum.Green => SKColors.Green,
            ColorEnum.Black => SKColors.Black,
            ColorEnum.Gray => SKColors.Gray,
            ColorEnum.Blue => SKColors.Blue,
            ColorEnum.White => SKColors.White,
            _ => GetColor((ColorEnum)Random.Next(1, 7)),
        };

    /// <summary>
    /// Parses "#rgb", "#rgba", "#rrggbb" or "#rrggbbaa" (leading '#' optional).
    /// Alpha is the LAST component, as in CSS (and as before with ImageSharp) — not ARGB like SKColor.Parse.
    /// </summary>
    public static SKColor GetColor(string hex)
    {
        var value = hex.Trim().TrimStart('#');
        if (value.Length is 3 or 4)
            value = string.Concat(value.Select(c => new string(c, 2)));
        if (value.Length == 6)
            value += "FF";
        if (value.Length != 8 || !uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgba))
            throw new ArgumentException($"Invalid hex color '{hex}'.", nameof(hex));

        return new SKColor((byte)(rgba >> 24), (byte)(rgba >> 16), (byte)(rgba >> 8), (byte)rgba);
    }
    public static SKColor GetColor(byte r, byte g, byte b) => new(r, g, b);
    public static SKColor GetColor(byte r, byte g, byte b, byte a) => new(r, g, b, a);
}