using System.Collections.Concurrent;
using SixLabors.Fonts;

namespace WonderfulCaptcha.Images;

public class FontUtils
{
    private static readonly Random Random = new();
    private static readonly ConcurrentDictionary<FontEnum, FontFamily> FontFamilies = new();

    public static Font GetFont(FontEnum fontFamily, int fontSize, FontStyleEnum style)
        => new(GetFontFamily(fontFamily), fontSize, GetFontStyle(style));

    public static FontFamily GetFontFamily(FontEnum font)
        => font switch
        {
            FontEnum.Arial => FontFamilies.GetOrAdd(font, _ => LoadFontFamily("ARIAL.TTF")),
            FontEnum.Tarsica => FontFamilies.GetOrAdd(font, _ => LoadFontFamily("TARSICA.TTF")),
            _ => GetFontFamily((FontEnum)Random.Next(1, 3)),
        };

    private static FontFamily LoadFontFamily(string fileName)
    {
        var fontPath = Path.Combine(AppContext.BaseDirectory, "Fonts", fileName);
        if (!File.Exists(fontPath))
            return SystemFonts.Get("Arial");

        var fontCollection = new FontCollection();
        return fontCollection.Add(fontPath);
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
