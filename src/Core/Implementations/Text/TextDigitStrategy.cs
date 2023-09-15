namespace WonderfulCaptcha.Text;
public class TextDigitStrategy : ITextStrategy
{
    private static readonly Random random = new Random();
    public string GetText(int len)
        => new string(Enumerable.Repeat("0123456789", len)
            .Select(s => s[random.Next(s.Length)]).ToArray());
}

