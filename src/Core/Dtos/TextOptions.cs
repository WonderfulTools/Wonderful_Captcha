namespace WonderfulCaptcha;

public class TextOptions
{
    public StrategyEnum Strategy { get; set; } = StrategyEnum.Character;
    public string Text { get; set; } = default!;
    public string Value { get; set; } = default!;
    public ColorEnum TextColor { get; set; } = ColorEnum.Random;
    public string TextColorHex { get; set; } = default!;
    public FontEnum TextFont { get; set; } = FontEnum.Random;
    public FontStyleEnum TextFontStyle { get; set; } = FontStyleEnum.Random;
    public BrushEnum TextBrush { get; set; } = BrushEnum.Solid;
    public (int Min, int Max) TextLen { get; set; } = (5, 5);
    public int FontSize { get; set; } = 50;
    public int FontSizeVarietyRange { get; set; } = 5;
    public bool TextShadow { get; set; } = false;
    public int TextRotationRange { get; set; } = 12;
    public int TextSkewRange { get; set; } = 10;
}
