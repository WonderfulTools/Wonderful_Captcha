namespace WonderfulCaptcha.Crypto;

public interface ICryptoEngine
{
    string Encrypt(string text);
    string Decrypt(string text);
}

