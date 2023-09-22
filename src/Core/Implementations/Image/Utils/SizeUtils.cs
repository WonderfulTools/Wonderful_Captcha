using SixLabors.Fonts;
using WonderfulCaptcha.Images.Settings;

namespace WonderfulCaptcha.Images;

public static class SizeUtils
{
    public static (int Width, int Height) GetDynamicSize(float fontSize, CaptchaOptions options)
    {
        Font font = new Font(SystemFonts.Get(FontSettings.DefaultFont), options.FontSize, FontSettings.DefaultFontStyle);
        return GetDynamicSize(font, options);
    }
    
    public static (int Width, int Height) GetDynamicSize(Font font, CaptchaOptions options)
    {
        var charSize = TextMeasurer.MeasureSize("A", new TextOptions(font));
        
        var width =
            (options.TextLen.Max * charSize.Width) +
            (options.TextLen.Max + 1) * options.CharSpacing +
            (2 * options.CharPositionVarietyRange.Width);

        var height =
            charSize.Height +
            2 * options.CharPositionVarietyRange.Height;

        return ((int)width, (int)height);
    }

    public static (int Width, int Height) GetRelativeFitSize((int Width, int Height) fitSize, float fontSize, CaptchaOptions options)
    {
        var dynamicSize = GetDynamicSize(fontSize, options);

        var relativeDifference =
            (dynamicSize.Width * dynamicSize.Width + dynamicSize.Height + dynamicSize.Height) -
            (fitSize.Width * fitSize.Width + fitSize.Height * fitSize.Height);
        
        var relativeThreshold = options.RelativeFitSizeThreshold * options.RelativeFitSizeThreshold;

        if (relativeDifference <= 0)
            return fitSize;

        return relativeDifference < relativeThreshold ? dynamicSize : fitSize;
    }

    public static int GetFitFontSize((int Width, int Height) fitSize, CaptchaOptions options)
    {
        var fontSize = options.FontSize;
        do
        {
            var dynamicSize = GetDynamicSize(fontSize, options);
            if (dynamicSize.Width <= fitSize.Width && dynamicSize.Height <= fitSize.Height)
                return fontSize;
            fontSize--;
        } while (fontSize > 0);

        return fontSize;
    }
}