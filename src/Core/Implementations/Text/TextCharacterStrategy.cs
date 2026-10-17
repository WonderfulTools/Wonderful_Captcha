using System.Security.Cryptography;

namespace WonderfulCaptcha.Text;
public class TextCharacterStrategy : ITextStrategy
{
    // Excludes look-alike glyphs (I/l/1, O/o/0) that users cannot tell apart once the text is rotated/skewed.
    // Verification is case-insensitive, so letters that differ only by case are safe.
    private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";

    public TextProviderResultDto GetText(int len)
    {
        var value = RandomNumberGenerator.GetString(Alphabet, len);
        return new TextProviderResultDto(value, value);
    }
}

