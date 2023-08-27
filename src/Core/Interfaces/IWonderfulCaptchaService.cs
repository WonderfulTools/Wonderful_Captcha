using Core.Enums;

namespace Core.Interfaces;


public interface ICaptchaPerBuilder
{
    ICaptchaBuilder WithStrategy(StrategyEnum strategy, CancellationToken cancellationToken = default);

}
public interface ICaptchaBuilder : ICaptchaPerBuilder
{
    ICaptchaBuilder WithComplexity(StrategyEnum complexity, CancellationToken cancellationToken = default);
    ICaptchaBuilder WithSize(int height = 10, int width = 20, CancellationToken cancellationToken = default);
    ICaptchaBuilder WithCaptchaText(string text, CancellationToken cancellationToken = default);
}
public interface ICaptchaFinisher
{
    object Generate();
    object GenerateAsync();
}

