namespace WonderfulCaptcha.Text;
public class TextFactory : ITextFactory
{
    public ITextStrategy GetInstance(StrategyEnum type)
        => type switch
        {
            StrategyEnum.Digits => new TextDigitStrategy(),
            StrategyEnum.Character => new TextCharacterStrategy(),
            _ => throw new InvalidOperationException($"Service of type {type} is not implemented."),
        };
}

