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
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithColor(string text)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithComplexity(StrategyEnum complexity)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithLen(int len)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithSize(int height = 10, int width = 20)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithStrategy(StrategyEnum strategy)
    {
        throw new NotImplementedException();
    }
}
