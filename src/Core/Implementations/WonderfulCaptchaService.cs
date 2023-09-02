using Core.Enums;
using Core.Interfaces;

namespace Core.Implementations;
public class WonderfulCaptchaService : ICaptchaBuilder, IOptionalCaptchaBuilder
{
    private string _text = default!;
    private int _height, _width;
    private StrategyEnum _strategy;
    private StrategyEnum _complexity;
    public WonderfulCaptchaService()
    {

    }
    public IOptionalCaptchaBuilder WithStrategy(StrategyEnum strategy, CancellationToken cancellationToken = default)
    {
        _strategy = strategy;
        return this;
    }
    public IOptionalCaptchaBuilder WithSize(int height = 10, int width = 20, CancellationToken cancellationToken = default)
    {
        _height = height;
        _width = width;
        return this;
    }
    public IOptionalCaptchaBuilder WithComplexity(StrategyEnum complexity, CancellationToken cancellationToken = default)
    {
        _complexity = complexity;
        return this;
    }
    public IOptionalCaptchaBuilder WithCaptchaText(string text, CancellationToken cancellationToken = default)
    {
        _text = text;
        return this;
    }

    public object Generate() => throw new NotImplementedException();
    public object GenerateAsync() => throw new NotImplementedException();
}

