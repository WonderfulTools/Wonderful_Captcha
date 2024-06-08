namespace WonderfulCaptcha.Text;
public class TextDigitStrategy : ITextStrategy
{
    private static readonly Random random = new();
    public TextProviderResultDto GetText(int len)
    {
        string value = new(Enumerable.Repeat("0123456789", len).Select(s => s[random.Next(s.Length)]).ToArray());
        return new TextProviderResultDto(value, value);
    }
}