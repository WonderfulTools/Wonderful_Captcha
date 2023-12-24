namespace WonderfulCaptcha.Text;
public class TextDigitStrategy : ITextStrategy
{
    private static readonly Random random = new();
    public string GetText(int len)
        => new(Enumerable.Repeat("0123456789", len)
            .Select(s => s[random.Next(s.Length)]).ToArray());
}