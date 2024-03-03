namespace WonderfulCaptcha.Text;
public class SimpleMathStrategy : ITextStrategy
{
    private static readonly Random random = new();
    public TextStrategyResultDto GetText(int len)
    {
        string oprand = new(Enumerable.Repeat("+-/*", 1).Select(s => s[random.Next(s.Length)]).ToArray());
        var number1 = random.Next(10, 20);
        var number2 = random.Next(1, 9);
        var text = $"{number1}{oprand}{number2}";
        var value = $"{number1 + number2}";
        switch (oprand)
        {
            case "+":
                value = $"{number1 + number2}";
                break;
            case "-":
                value = $"{number1 - number2}";
                break;
            case "/":
                value = $"{number1 / number2}";
                break;
            case "*":
                value = $"{number1 * number2}";
                break;
            default:
                break;
        }
        return new TextStrategyResultDto(text, value);
    }
}

