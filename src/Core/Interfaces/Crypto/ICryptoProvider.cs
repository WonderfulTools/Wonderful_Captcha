namespace WonderfulCaptcha.Crypto;

public interface ICryptoProvider
{
    string Encrypt(string text);
    string Decrypt(string text);
}

