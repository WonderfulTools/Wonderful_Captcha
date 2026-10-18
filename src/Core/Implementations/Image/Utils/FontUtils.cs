using System.Collections.Concurrent;
using SixLabors.Fonts;

namespace WonderfulCaptcha.Images;

public class FontUtils
{
    private static readonly Random Random = Random.Shared;
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
        // Fonts are embedded in the assembly so no OS fonts are required (e.g. Alpine containers).
        var resourceName = $"WonderfulCaptcha.Fonts.{fileName}";
        using var stream = typeof(FontUtils).Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded font resource '{resourceName}' not found.");

        return new FontCollection().Add(stream);
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
