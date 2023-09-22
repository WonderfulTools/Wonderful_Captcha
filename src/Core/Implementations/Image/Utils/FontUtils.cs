using SixLabors.Fonts;

namespace WonderfulCaptcha.Images;

public class FontUtils
{
    private static readonly Random Random = new Random();

    public static Font GetFont(FontEnum fontFamily, int fontSize, FontStyleEnum style)
        => new Font(GetFontFamily(fontFamily), fontSize, GetFontStyle(style));
    
    public static FontFamily GetFontFamily(FontEnum font)
        => font switch
        {
            FontEnum.Arial => SystemFonts.Get("Arial"),
            _ => GetFontFamily((FontEnum)Random.Next(8))
        };
    
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
        => TextMeasurer.MeasureSize("A", new TextOptions(font)).Width;

    public static float GetCharWidth(int fontSize)
    {
        var font = GetFont(FontEnum.Arial, fontSize, FontStyleEnum.Regular);
        return TextMeasurer.MeasureSize("A", new TextOptions(font)).Width;
    }
        
}
