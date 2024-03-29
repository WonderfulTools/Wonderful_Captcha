﻿using System.Security.Cryptography;
using System.Text;

namespace WonderfulCaptcha.Crypto;
public class SHA256CryptoEngine : ICryptoProvider
{
    private readonly string _key;
    public SHA256CryptoEngine()
    {
        _key = "PASS!@#$%^&*()_+PASS";
    }
    public string Decrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
            return default!;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Mode = CipherMode.CFB;
            aesAlg.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_key));
            aesAlg.IV = Convert.FromBase64String(text).Take(16).ToArray();

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new(Convert.FromBase64String(text).Skip(16).ToArray()))
            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }

    public string Encrypt(string text)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Mode = CipherMode.CFB;
            aesAlg.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_key));
            aesAlg.IV = new byte[16];

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new())
            {
                using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new(csEncrypt))
                    swEncrypt.Write(text);

                return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
            }
        }
    }
}

