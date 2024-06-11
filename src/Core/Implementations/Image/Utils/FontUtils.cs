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
        FontFamily ArialFontFamily = fontCollection.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Fonts\ARIAL.TTF");

        return font switch
        {
            FontEnum.Arial => ArialFontFamily,
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
