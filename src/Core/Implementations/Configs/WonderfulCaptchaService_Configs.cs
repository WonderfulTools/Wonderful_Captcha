namespace WonderfulCaptcha;

public partial class WonderfulCaptchaService
{
    public IWonderfulCaptchaService WithTextColor(ColorEnum color) { _options.TextColor = color; return this; }
    public IWonderfulCaptchaService WithTextColor(string colorHex) { _options.TextColorHex = colorHex; return this; }
    public IWonderfulCaptchaService WithTextFont(FontEnum font) { _options.TextFont = font; return this; }
    public IWonderfulCaptchaService WithTextFontStyle(FontStyleEnum fontStyle) { _options.TextFontStyle = fontStyle; return this; }
    public IWonderfulCaptchaService WithTextBrush(BrushEnum brush) { _options.TextBrush = brush; return this; }
    public IWonderfulCaptchaService WithTextLength(int min, int max) { _options.TextLen = (min, max); return this; }
    public IWonderfulCaptchaService WithTextFontSize(int fontSize) { _options.FontSize = fontSize; return this; }
    public IWonderfulCaptchaService WithTextFontSizeVarietyRange(int varietyRange) { _options.FontSizeVarietyRange = varietyRange; return this; }
    public IWonderfulCaptchaService WithTextShadow(bool haveShadow = true) { _options.TextShadow = haveShadow; return this; }
    public IWonderfulCaptchaService WithTextRotationRange(int textRotationRange) { _options.TextRotationRange = textRotationRange; return this; }
    public IWonderfulCaptchaService WithTextSkewRange(int textSkewRange) { _options.TextSkewRange = textSkewRange; return this; }


    public IWonderfulCaptchaService WithImageSize(int width, int height) { _options.ImageSize = (width, height); return this; }
    public IWonderfulCaptchaService WithImageSizeStrategy(ImageSizeStrategy imageSizeStrategy) { _options.ImageSizeStrategy = imageSizeStrategy; return this; }

    public IWonderfulCaptchaService WithImageBackgroundColor(ColorEnum color) { _options.ImageBackgroundColor = color; return this; }
    public IWonderfulCaptchaService WithImageBackgroundColorHex(string colorHex) { _options.ImageBackgroundColorHex = colorHex; return this; }
    public IWonderfulCaptchaService WithRelativeFitSizeThreshold(int relativeFitSizeThreshold) { _options.RelativeFitSizeThreshold = relativeFitSizeThreshold; return this; }
    public IWonderfulCaptchaService WithCharSpacing(int charSpacing) { _options.CharSpacing = charSpacing; return this; }
    public IWonderfulCaptchaService WithCharPositionVarietyRange(int width, int height) { _options.CharPositionVarietyRange = (width, height); return this; }


    public IWonderfulCaptchaService WithComplexity(int complexity) { _options.Complexity = complexity; return this; }
    public IWonderfulCaptchaService WithStrategy(StrategyEnum strategy) { _options.Strategy = strategy; return this; }

}
