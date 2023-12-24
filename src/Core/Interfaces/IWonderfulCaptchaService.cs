namespace WonderfulCaptcha;

public interface IWonderfulCaptchaService
{
    CaptchaResult Generate(CaptchaOptions options = default!);
    Task<CaptchaResult> GenerateAsync(Action<CaptchaOptions> options = default!, CancellationToken cancellationToken = default);
    bool Verify(string key, string value);
    Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default);
}

