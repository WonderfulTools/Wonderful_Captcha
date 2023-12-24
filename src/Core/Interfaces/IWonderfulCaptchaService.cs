namespace WonderfulCaptcha;

public interface IWonderfulCaptchaService
{
    Task<CaptchaResult> GenerateAsync(Action<CaptchaOptions> options = default!, CancellationToken cancellationToken = default);
    Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default);
}

