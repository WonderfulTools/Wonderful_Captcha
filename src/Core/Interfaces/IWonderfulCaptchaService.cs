namespace WonderfulCaptcha;

public partial interface IWonderfulCaptchaService
{
    string Generate();
    Task<CaptchaResult> GenerateAsync(CancellationToken cancellationToken = default);
    bool Verify(string key, string value);
    Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default);
}

