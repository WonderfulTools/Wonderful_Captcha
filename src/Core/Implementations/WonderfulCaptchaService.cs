using WonderfulCaptcha.Cache;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;

public class WonderfulCaptchaService : IWonderfulCaptchaService
{
    private CaptchaOptions _options;
    private ITextProvider _textProvider;
    private ICacheProvider _cacheProvider;
    private ICryptoProvider _cryptoProvider;
    private IImageGenerator _imageGenerator;

    public WonderfulCaptchaService(IOptions<CaptchaOptions> captchaOptions,
                                   ITextProvider textProvider,
                                   ICacheProvider cacheProvider,
                                   ICryptoProvider cryptoProvider,
                                   IImageGenerator imageGenerator)
    {
        _options = captchaOptions.Value;
        _textProvider = textProvider;
        _cacheProvider = cacheProvider;
        _cryptoProvider = cryptoProvider;
        _imageGenerator = imageGenerator;
    }

    public async Task<CaptchaResult> GenerateAsync(Action<CaptchaOptions> options = default!, CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();

        if (options is not null)
            options.Invoke(_options);

        var textResult = _textProvider.GetInstance(_options.TextOptions.Strategy)
            .GetText(Helpers.GetRandomNumberBetween(_options.TextOptions.TextLen.Min, _options.TextOptions.TextLen.Max));
        _options.TextOptions.Text = textResult.Text;
        _options.TextOptions.Value = textResult.Value;

        await _cacheProvider.SetAsync(key, _cryptoProvider.Encrypt(_options.TextOptions.Value), _options.CacheOptions.CacheExpirationTime, cancellationToken);

        var image = await _imageGenerator.GenerateImageAsync(cancellationToken);
        return new CaptchaResult(key, image);
    }

    public bool Verify(string key, string value)
    {
        var cachedValue = _cacheProvider.GetAsync<string>(key).Result;
        if (cachedValue is null)
            return false;
        _cacheProvider.RemoveAsync(key);
        return _cryptoProvider.Decrypt(cachedValue).Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _cacheProvider.GetAsync<string>(key, cancellationToken);
        if (cachedValue is null)
            return false;
        await _cacheProvider.RemoveAsync(key, cancellationToken);
        return _cryptoProvider.Decrypt(cachedValue).Equals(value, StringComparison.OrdinalIgnoreCase);
    }
}

