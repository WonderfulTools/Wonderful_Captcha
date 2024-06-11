using SixLabors.Fonts;
using System.Reflection;

namespace WonderfulCaptcha.Images;

public class FontUtils
{
    private static readonly Random Random = new();

    public static Font GetFont(FontEnum fontFamily, int fontSize, FontStyleEnum style)
        => new(GetFontFamily(fontFamily), fontSize, GetFontStyle(style));

    public static FontFamily GetFontFamily(FontEnum font)
    {
        FontCollection fontCollection = new();
        string fontsDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Fonts");

        return font switch
        {
            FontEnum.Arial => fontCollection.Add(Path.Combine(fontsDirectory, @"ARIAL.TTF")),
            FontEnum.Tarsica => fontCollection.Add(Path.Combine(fontsDirectory, @"TARSICA.TTF")),
            _ => GetFontFamily((FontEnum)Random.Next(8))
        };
    }


    public static FontStyle GetFontStyle(FontStyleEnum style)
        => style switch
        {
            FontStyleEnum.Italic => FontStyle.Italic,
            FontStyleEnum.Bold => FontStyle.Bold,
            FontStyleEnum.BoldItalic => FontStyle.BoldItalic,
            FontStyleEnum.Regular => FontStyle.Regular,
            _ => GetFontStyle((FontStyleEnum)Random.Next(4)),
        };

    public static float GetCharWidth(Font font)
        => TextMeasurer.MeasureSize("A", new SixLabors.Fonts.TextOptions(font)).Width;

    public static float GetCharWidth(int fontSize)
    {
        var font = GetFont(FontEnum.Arial, fontSize, FontStyleEnum.Regular);
        return TextMeasurer.MeasureSize("A", new SixLabors.Fonts.TextOptions(font)).Width;
    }

}
