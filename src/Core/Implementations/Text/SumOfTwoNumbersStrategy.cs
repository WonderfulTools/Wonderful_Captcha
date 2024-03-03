namespace WonderfulCaptcha.Text;
public class SumOfTwoNumbersStrategy : ITextStrategy
{
    private static readonly Random random = new();
    public TextStrategyResultDto GetText(int len)
    {
        var number1 = random.Next(10, 20);
        var number2 = random.Next(1, 9);
        var text = $"{number1}+{number2}";
        var value = $"{number1 + number2}";
        return new TextStrategyResultDto(text, value);
    }
}

