using System.Security.Cryptography;

namespace WonderfulCaptcha.Text;
public class TextCharacterStrategy : ITextStrategy
{
    public TextProviderResultDto GetText(int len)
    {
        var rng = RandomNumberGenerator.Create();
        byte[] rno = new byte[len];
        rng.GetBytes(rno);
        var value = Convert.ToBase64String(rno).Replace("+", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty)[..len];
        return new TextProviderResultDto(value, value);
    }
}

