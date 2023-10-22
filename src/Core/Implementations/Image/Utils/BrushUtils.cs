using SixLabors.ImageSharp.Drawing.Processing;

namespace WonderfulCaptcha.Images;

public class BrushUtils
{
    private static readonly Random Random = new();

    public static Brush GetBrush(BrushEnum brush, Color color) =>
        brush switch
        {
            BrushEnum.Percent20 => Brushes.Percent20(color),
            BrushEnum.BackwardDiagonal => Brushes.BackwardDiagonal(color),
            BrushEnum.ForwardDiagonal => Brushes.ForwardDiagonal(color),
            BrushEnum.Horizontal => Brushes.Horizontal(color),
            BrushEnum.Min => Brushes.Min(color),
            BrushEnum.Vertical => Brushes.Vertical(color),
            BrushEnum.Solid => Brushes.Solid(color),
            _ => GetBrush((BrushEnum)Random.Next(9), color),
        };
}