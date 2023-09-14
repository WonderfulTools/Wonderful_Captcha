using Core.Enums;

namespace Core.Interfaces;

public interface IWonderfulCaptchaService
{
    IWonderfulCaptchaService WithStrategy(StrategyEnum strategy);
    IWonderfulCaptchaService WithComplexity(StrategyEnum complexity);
    IWonderfulCaptchaService WithSize(int height = 10, int width = 20);
    IWonderfulCaptchaService WithCaptchaText(string text);
    IWonderfulCaptchaService WithLen(int len);
    IWonderfulCaptchaService WithColor(string text);
    IWonderfulCaptchaService WithBackGroundColor(string text);
    string Generate();
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
    bool Verify(string key, string value);
    Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default);
}

