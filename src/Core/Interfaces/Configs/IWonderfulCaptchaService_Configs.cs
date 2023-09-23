namespace WonderfulCaptcha;

public partial interface IWonderfulCaptchaService
{
    IWonderfulCaptchaService WithStrategy(StrategyEnum strategy);
    IWonderfulCaptchaService WithComplexity(int complexity);
    IWonderfulCaptchaService WithSize(int height = 10, int width = 20);
    IWonderfulCaptchaService WithCaptchaText(string text);
    IWonderfulCaptchaService WithLen(int min, int max);
    IWonderfulCaptchaService WithColor(string text);
    IWonderfulCaptchaService WithBackGroundColor(string text);
}

