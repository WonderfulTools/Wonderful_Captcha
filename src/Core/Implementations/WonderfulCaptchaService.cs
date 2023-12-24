using Microsoft.Extensions.Options;
using WonderfulCaptcha.Cache;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;

public class WonderfulCaptchaService : IWonderfulCaptchaService
{
    private readonly CaptchaOptions _options;
    private readonly Lazy<ITextProvider> _textProvider;
    private readonly ICacheProvider _cacheProvider;
    private readonly Lazy<ICryptoProvider> _cryptoProvider;
    private readonly Lazy<IImageGenerator> _imageGenerator;

    public WonderfulCaptchaService(
        IOptions<CaptchaOptions> captchaOptions,
        Lazy<ITextProvider> textProvider,
        ICacheProvider cacheProvider,
        Lazy<ICryptoProvider> cryptoProvider,
        Lazy<IImageGenerator> imageGenerator)
    {
        _options = captchaOptions.Value;
        _textProvider = textProvider;
        _cacheProvider = cacheProvider;
        _cryptoProvider = cryptoProvider;
        _imageGenerator = imageGenerator;
    }

    public async Task<CaptchaResult> GenerateAsync(Action<CaptchaOptions> options = default!,
        CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();

        options.Invoke(_options);

        _options.TextOptions.Text = _textProvider.Value.GetInstance(_options.TextOptions.Strategy)
            .GetText(Helpers.GetRandomNumberBetween(_options.TextOptions.TextLen.Min,
                _options.TextOptions.TextLen.Max));

        await _cacheProvider.SetAsync(key, _cryptoProvider.Value.Encrypt(_options.TextOptions.Text),
            _options.CacheOptions.CacheExpirationTime, cancellationToken);

        var image = await _imageGenerator.Value.GenerateImageAsync(cancellationToken);
        return new CaptchaResult(key, image);
    }
    
    public async Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _cacheProvider.GetAsync<string>(key, cancellationToken);
        await _cacheProvider.RemoveAsync(key, cancellationToken);
        return _cryptoProvider.Value.Decrypt(cachedValue).Equals(value, StringComparison.OrdinalIgnoreCase);
    }
}