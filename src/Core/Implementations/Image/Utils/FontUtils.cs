using System.Collections.Concurrent;

namespace WonderfulCaptcha.Images;

public class FontUtils
{
    private static readonly Random Random = Random.Shared;
    private static readonly ConcurrentDictionary<FontEnum, SKTypeface> FontFamilies = new();

    /// <summary>
    /// Creates a font for drawing/measuring. The caller owns (and must dispose) the returned <see cref="SKFont"/>.
    /// The embedded font files only contain a regular face, so <paramref name="style"/> does not change the glyphs
    /// (same behaviour as the previous ImageSharp renderer, which fell back to the regular face).
    /// </summary>
    public static SKFont GetFont(FontEnum fontFamily, int fontSize, FontStyleEnum style)
    {
        _ = GetFontStyle(style);
        return new SKFont(GetFontFamily(fontFamily), fontSize)
        {
            Edging = SKFontEdging.Antialias,
            Hinting = SKFontHinting.None,
            Subpixel = true,
        };
    }

    public static SKTypeface GetFontFamily(FontEnum font)
        => font switch
        {
            FontEnum.Arial => FontFamilies.GetOrAdd(font, _ => LoadFontFamily("ARIAL.TTF")),
            FontEnum.Tarsica => FontFamilies.GetOrAdd(font, _ => LoadFontFamily("TARSICA.TTF")),
            _ => GetFontFamily((FontEnum)Random.Next(1, 3)),
        };

    private static SKTypeface LoadFontFamily(string fileName)
    {
        // Fonts are embedded in the assembly so no OS fonts are required (e.g. Alpine containers).
        var resourceName = $"WonderfulCaptcha.Fonts.{fileName}";
        using var stream = typeof(FontUtils).Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded font resource '{resourceName}' not found.");

        using var data = SKData.Create(stream);
        return SKTypeface.FromData(data)
            ?? throw new InvalidOperationException($"Embedded font resource '{resourceName}' could not be loaded.");
    }


    public static SKFontStyle GetFontStyle(FontStyleEnum style)
        => style switch
        {
            FontStyleEnum.Italic => SKFontStyle.Italic,
            FontStyleEnum.Bold => SKFontStyle.Bold,
            FontStyleEnum.BoldItalic => SKFontStyle.BoldItalic,
            FontStyleEnum.Regular => SKFontStyle.Normal,
            _ => GetFontStyle((FontStyleEnum)Random.Next(4)),
        };

    /// <summary>Exact size of the inked area (glyph outlines) of <paramref name="text"/>, not the advance/line box.</summary>
    public static SKSize MeasureSize(string text, SKFont font)
    {
        using var path = font.GetTextPath(text, SKPoint.Empty);
        return path.TightBounds.Size;
    }

    /// <summary>
    /// Distance from the top of the layout box to the baseline. Drawing positions in this library refer to the
    /// top-left of the text box (ImageSharp convention), while Skia draws text at the baseline.
    /// The line box is one em high and the hhea ascender/descender are centred in it.
    /// </summary>
    public static float GetBaselineOffset(SKFont font)
    {
        var (ascender, descender) = VerticalMetrics.GetOrAdd(font.Typeface, ReadVerticalMetrics);
        var ascent = ascender * font.Size;
        var descent = -descender * font.Size;
        return (font.Size - (ascent + descent)) / 2 + ascent;
    }

    private static readonly ConcurrentDictionary<SKTypeface, (float Ascender, float Descender)> VerticalMetrics = new();

    // hhea ascender/descender in em units. Skia's own SKFontMetrics may come from the OS/2 table instead,
    // which places some fonts (e.g. Tarsica) a few pixels lower than before.
    private static (float Ascender, float Descender) ReadVerticalMetrics(SKTypeface typeface)
    {
        const uint hhea = 0x68686561; // 'hhea'
        if (typeface.TryGetTableData(hhea, out var table) && table.Length >= 8 && typeface.UnitsPerEm > 0)
        {
            var ascender = (short)((table[4] << 8) | table[5]);
            var descender = (short)((table[6] << 8) | table[7]);
            return ((float)ascender / typeface.UnitsPerEm, (float)descender / typeface.UnitsPerEm);
        }

        using var font = new SKFont(typeface, 1);
        return (-font.Metrics.Ascent, -font.Metrics.Descent);
    }

    public static float GetCharWidth(SKFont font)
        => MeasureSize("A", font).Width;

    public static float GetCharWidth(int fontSize)
    {
        using var font = GetFont(FontEnum.Arial, fontSize, FontStyleEnum.Regular);
        return GetCharWidth(font);
    }

}
