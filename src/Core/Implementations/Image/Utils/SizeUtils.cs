using SixLabors.Fonts;
using WonderfulCaptcha.Images.Settings;

namespace WonderfulCaptcha.Images;

public static class SizeUtils
{
    public static (int Width, int Height) GetDynamicSize(float fontSize, CaptchaOptions options)
    {
        Font font = new(SystemFonts.Get(FontSettings.DefaultFont), fontSize, FontSettings.DefaultFontStyle);
        var charSize = TextMeasurer.MeasureSize("A", new SixLabors.Fonts.TextOptions(font));

        var width =
            (options.TextOptions.Text.Length * charSize.Width) +
            (options.TextOptions.Text.Length + 1) * options.ImageOptions.CharSpacing +
            (2 * options.ImageOptions.CharPositionVarietyRange.Width);

        var height =
            charSize.Height +
            2 * options.ImageOptions.CharPositionVarietyRange.Height;

        return ((int)width, (int)height);
    }
    public static (int Width, int Height) GetRelativeFitSize(float fontSize, CaptchaOptions options)
    {
        var dynamicSize = GetDynamicSize(fontSize, options);
        var fitSize = options.ImageOptions.ImageSize;

        var relativeDifference =
            (dynamicSize.Width * dynamicSize.Width + dynamicSize.Height + dynamicSize.Height) -
            (fitSize.Width * fitSize.Width + fitSize.Height * fitSize.Height);

        var relativeThreshold = options.ImageOptions.RelativeFitSizeThreshold * options.ImageOptions.RelativeFitSizeThreshold;

        if (relativeDifference <= 0)
            return dynamicSize;

        return relativeDifference < relativeThreshold ? dynamicSize : fitSize;
    }


    public static int GetFitFontSize(CaptchaOptions options)
    {
        var fontSize = options.TextOptions.FontSize;
        do
        {
            var dynamicSize = GetDynamicSize(fontSize, options);
            if (dynamicSize.Width <= options.ImageOptions.ImageSize.Width)
                return fontSize;
            fontSize--;
        } while (fontSize > 0);

        return fontSize;
    }
    public static int GetRelativeFitFontSize(CaptchaOptions options)
    {
        var dynamicSize = GetDynamicSize(options.TextOptions.FontSize, options);
        var fitSize = options.ImageOptions.ImageSize;

        var relativeDifference =
            (dynamicSize.Width * dynamicSize.Width + dynamicSize.Height + dynamicSize.Height) -
            (fitSize.Width * fitSize.Width + fitSize.Height * fitSize.Height);

        var relativeThreshold = options.ImageOptions.RelativeFitSizeThreshold * options.ImageOptions.RelativeFitSizeThreshold;

        var fitFontSize = GetFitFontSize(options);
        var dynamicFontSize = options.TextOptions.FontSize;

        if (relativeDifference <= 0)
            return dynamicFontSize;

        return relativeDifference < relativeThreshold ? dynamicFontSize : fitFontSize;
    }


}