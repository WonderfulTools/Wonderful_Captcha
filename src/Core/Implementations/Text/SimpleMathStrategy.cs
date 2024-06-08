namespace WonderfulCaptcha.Text;
public class SimpleMathStrategy : ITextStrategy
{
    private static readonly Random random = new();
    public TextProviderResultDto GetText(int len)
    {
        var numbers = new List<int> { random.Next(10, 20), random.Next(1, 9) };
        numbers = numbers.OrderBy(x => random.Next()).ToList();

        var number1 = numbers[0];
        var number2 = numbers[1];

        bool useAddition = random.Next(2) == 0;

        string text = useAddition ? $"{number1}+{number2}" : $"{Math.Max(number1, number2)}-{Math.Min(number1, number2)}";
        string value = useAddition ? $"{number1 + number2}" : $"{Math.Abs(number1 - number2)}";

        return new TextProviderResultDto(text, value);
    }
}

