namespace WonderfulCaptcha.Text;
public class SumOfTwoNumbersStrategy : ITextStrategy
{
    private static readonly Random random = new();
    public TextStrategyResultDto GetText(int len)
    {

        var numbers = new List<int> { random.Next(10, 20), random.Next(1, 9) };
        numbers = numbers.OrderBy(x => random.Next()).ToList();
        var number1 = numbers[0];
        var number2 = numbers[1];

        var text = $"{number1}+{number2}";
        var value = $"{number1 + number2}";
        return new TextStrategyResultDto(text, value);
    }
}

