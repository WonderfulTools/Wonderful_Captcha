namespace WonderfulCaptcha;

public interface IWonderfulCaptchaService
{
    IWonderfulCaptchaService WithStrategy(StrategyEnum strategy);
    IWonderfulCaptchaService WithComplexity(StrategyEnum complexity);
    IWonderfulCaptchaService WithSize(int height = 10, int width = 10);
    IWonderfulCaptchaService WithCaptchaText(string text);
    IWonderfulCaptchaService WithLen(int len);
    IWonderfulCaptchaService WithColor(string text);
    IWonderfulCaptchaService WithBackGroundColor(string text);
    string Generate();
    Task<CaptchaResult> GenerateAsync(CancellationToken cancellationToken = default);
    Task<CaptchaResult> GenerateDigitsAsync(CancellationToken cancellationToken = default);
    Task<CaptchaResult> GenerateLettersAsync(CancellationToken cancellationToken = default);
    bool Verify(string key, string value);
    Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default);
}

