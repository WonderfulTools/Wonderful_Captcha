using SixLabors.Fonts;

namespace WonderfulCaptcha.Images;

public class FontUtils
{
    private static readonly Random Random = new();

    public static Font GetFont( int fontSize, FontStyleEnum style, Font customFont)
        => new(customFont.Family, fontSize, GetFontStyle(style));

    public static FontStyle GetFontStyle(FontStyleEnum style)
        => style switch
        {
            FontStyleEnum.Italic => FontStyle.Italic,
            FontStyleEnum.Bold => FontStyle.Bold,
            FontStyleEnum.BoldItalic => FontStyle.BoldItalic,
            FontStyleEnum.Regular => FontStyle.Regular,
            _ => GetFontStyle((FontStyleEnum)Random.Next(4)),
        };

    public static float GetCharWidth(Font font, int fontSize)
        => TextMeasurer.MeasureSize("A", new SixLabors.Fonts.TextOptions(font)).Width;
}
