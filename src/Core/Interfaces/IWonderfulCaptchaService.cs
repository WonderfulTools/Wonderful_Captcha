using Core.Enums;

namespace Core.Interfaces;


public interface ICaptchaBuilder
{
    IOptionalCaptchaBuilder WithStrategy(StrategyEnum strategy, CancellationToken cancellationToken = default);

}
public interface IOptionalCaptchaBuilder : ICaptchaBuilder
{
    IOptionalCaptchaBuilder WithComplexity(StrategyEnum complexity, CancellationToken cancellationToken = default);
    IOptionalCaptchaBuilder WithSize(int height = 10, int width = 20, CancellationToken cancellationToken = default);
    IOptionalCaptchaBuilder WithCaptchaText(string text, CancellationToken cancellationToken = default);
    IOptionalCaptchaBuilder WithLen(int len, CancellationToken cancellationToken = default);
    IOptionalCaptchaBuilder WithColor(string text, CancellationToken cancellationToken = default);
    IOptionalCaptchaBuilder WithBackGroundColor(string text, CancellationToken cancellationToken = default);
    object Generate();
    object GenerateAsync();
}

