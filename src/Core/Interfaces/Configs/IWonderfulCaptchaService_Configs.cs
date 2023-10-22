namespace WonderfulCaptcha;

public partial interface IWonderfulCaptchaService
{
    IWonderfulCaptchaService WithTextColor(ColorEnum color);
    IWonderfulCaptchaService WithTextColor(string colorHex);
    IWonderfulCaptchaService WithTextFont(FontEnum font);
    IWonderfulCaptchaService WithTextFontStyle(FontStyleEnum fontStyle);
    IWonderfulCaptchaService WithTextBrush(BrushEnum brush);
    IWonderfulCaptchaService WithTextLength(int min, int max);
    IWonderfulCaptchaService WithTextFontSize(int fontSize);
    IWonderfulCaptchaService WithTextFontSizeVarietyRange(int varietyRange);
    IWonderfulCaptchaService WithTextShadow(bool haveShadow = true);
    IWonderfulCaptchaService WithTextRotationRange(int textRotationRange);
    IWonderfulCaptchaService WithTextSkewRange(int textSkewRange);


    IWonderfulCaptchaService WithImageSize(int width, int height);
    IWonderfulCaptchaService WithImageSizeStrategy(ImageSizeStrategy imageSizeStrategy);
    IWonderfulCaptchaService WithImageBackgroundColor(ColorEnum color);
    IWonderfulCaptchaService WithImageBackgroundColorHex(string colorHex);
    IWonderfulCaptchaService WithRelativeFitSizeThreshold(int relativeFitSizeThreshold);
    IWonderfulCaptchaService WithCharSpacing(int charSpacing);
    IWonderfulCaptchaService WithCharPositionVarietyRange(int width, int height);


    IWonderfulCaptchaService WithComplexity(int complexity);
    IWonderfulCaptchaService WithStrategy(StrategyEnum strategy);
}

