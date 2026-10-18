namespace WonderfulCaptcha.Images;

public class BrushUtils
{
    private static readonly Random Random = Random.Shared;

    // Hatch patterns (rows = y, columns = x); set cells use the brush color, the rest is transparent.
    private static readonly bool[,] Percent20Pattern =
    {
        { true,  false, false, false },
        { false, false, true,  false },
        { true,  false, false, false },
        { false, false, true,  false },
    };
    private static readonly bool[,] HorizontalPattern = { { false }, { true }, { false }, { false } };
    private static readonly bool[,] MinPattern = { { false }, { false }, { false }, { true } };
    private static readonly bool[,] VerticalPattern = { { false, true, false, false } };
    private static readonly bool[,] ForwardDiagonalPattern =
    {
        { false, false, false, true  },
        { false, false, true,  false },
        { false, true,  false, false },
        { true,  false, false, false },
    };
    private static readonly bool[,] BackwardDiagonalPattern =
    {
        { true,  false, false, false },
        { false, true,  false, false },
        { false, false, true,  false },
        { false, false, false, true  },
    };

    /// <summary>
    /// Creates a fill paint for <paramref name="brush"/>. Pattern brushes are anchored to image pixels,
    /// so pass the canvas' current transform as <paramref name="canvasTransform"/> when drawing with one.
    /// </summary>
    public static SKPaint GetBrush(BrushEnum brush, SKColor color, SKMatrix? canvasTransform = null) =>
        brush switch
        {
            BrushEnum.Random => GetBrush((BrushEnum)Random.Next(15), color, canvasTransform),
            BrushEnum.Percent20 => Pattern(Percent20Pattern, color, canvasTransform),
            BrushEnum.BackwardDiagonal => Pattern(BackwardDiagonalPattern, color, canvasTransform),
            BrushEnum.ForwardDiagonal => Pattern(ForwardDiagonalPattern, color, canvasTransform),
            BrushEnum.Horizontal => Pattern(HorizontalPattern, color, canvasTransform),
            BrushEnum.Min => Pattern(MinPattern, color, canvasTransform),
            BrushEnum.Vertical => Pattern(VerticalPattern, color, canvasTransform),
            BrushEnum.Solid => Solid(color),
            _ => Solid(color),
        };

    private static SKPaint Solid(SKColor color)
        => new() { Color = color, IsAntialias = true, Style = SKPaintStyle.Fill };

    private static SKPaint Pattern(bool[,] pattern, SKColor color, SKMatrix? canvasTransform)
    {
        int rows = pattern.GetLength(0), cols = pattern.GetLength(1);
        using var tile = new SKBitmap(new SKImageInfo(cols, rows, SKColorType.Rgba8888, SKAlphaType.Premul));
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                tile.SetPixel(x, y, pattern[y, x] ? color : SKColors.Transparent);

        // Undo the canvas transform so the hatch stays aligned to the image grid instead of rotating with the glyph.
        var localMatrix = canvasTransform is { } m && m.TryInvert(out var inverse) ? inverse : SKMatrix.Identity;
        using var tileImage = SKImage.FromBitmap(tile);
        return new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = tileImage.ToShader(SKShaderTileMode.Repeat, SKShaderTileMode.Repeat,
                                        new SKSamplingOptions(SKFilterMode.Nearest), localMatrix),
        };
    }
}