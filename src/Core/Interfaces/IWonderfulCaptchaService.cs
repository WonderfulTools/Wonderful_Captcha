namespace Core.Interfaces;

public interface IWonderfulCaptchaService
{
    Task<string> GenerateCaptchaAsync(CancellationToken cancellationToken);
}

