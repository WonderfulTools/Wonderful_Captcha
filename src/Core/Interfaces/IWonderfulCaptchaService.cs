namespace WonderfulCaptcha;

public interface IWonderfulCaptchaService
{
    string Generate(CaptchaOptions options = default!);
    Task<CaptchaResult> GenerateAsync(CaptchaOptions options = default!, CancellationToken cancellationToken = default);
    bool Verify(string key, string value);
    Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default);
}

