using System.Security.Cryptography;

namespace WonderfulCaptcha.Text;
public class TextCharacterStrategy : ITextStrategy
{
    public string GetText(int len)
    {
        var rng = RandomNumberGenerator.Create();
        byte[] rno = new byte[len];
        rng.GetBytes(rno);
        return Convert.ToBase64String(rno).Replace("/", string.Empty).Replace(" ", string.Empty)[..len];
    }
}

