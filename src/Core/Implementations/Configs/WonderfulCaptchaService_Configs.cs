using static System.Net.Mime.MediaTypeNames;

namespace WonderfulCaptcha;

public partial class WonderfulCaptchaService
{
    public IWonderfulCaptchaService WithBackGroundColor(string text)
    {
        captchaOptions.Text = text;
        return this;
    }

    public IWonderfulCaptchaService WithCaptchaText(string text)
    {
        captchaOptions.Text = text;
        return this;
    }

    public IWonderfulCaptchaService WithColor(string text)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithComplexity(int complexity)
    {
        captchaOptions.Complexity = complexity;
        return this;
    }

    public IWonderfulCaptchaService WithLen(int min, int max)
    {
        captchaOptions.TextLen = (min, max);
        return this;
    }

    public IWonderfulCaptchaService WithSize(int height = 10, int width = 20)
    {
        captchaOptions.Size = (height, width);
        return this;
    }

    public IWonderfulCaptchaService WithStrategy(StrategyEnum strategy)
    {
        captchaOptions.Strategy = strategy;
        return this;
    }
}
