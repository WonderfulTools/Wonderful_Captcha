using WonderfulCaptcha.Text;
using WonderfulCaptcha.Cache;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;

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

    private string GetCacheWithPrefix(string key) => $"{_options.CacheOptions.Prefix}_{key}";

    public async Task<CaptchaResult> GenerateAsync(Action<CaptchaOptions> options = default!, CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();

        // _options is the singleton registered via IOptions and is shared by all concurrent requests.
        // Work on a private copy so the text/value of this captcha cannot be overwritten by another request.
        var callOptions = _options.Clone();
        if (options is not null)
            options.Invoke(callOptions);

        var textResult = _textProvider.GetInstance(callOptions.TextOptions.Strategy)
            .GetText(Helpers.GetRandomNumberBetween(callOptions.TextOptions.TextLen.Min, callOptions.TextOptions.TextLen.Max));
        callOptions.TextOptions.Text = textResult.Text;
        callOptions.TextOptions.Value = textResult.Value;

        await _cacheProvider.SetAsync(GetCacheWithPrefix(key), _cryptoProvider.Encrypt(textResult.Value), callOptions.CacheOptions.CacheExpirationTime, cancellationToken);

        var image = await _imageGenerator.GenerateImageAsync(callOptions, cancellationToken);
        return new CaptchaResult(key, image);
    }

    public bool Verify(string key, string value)
    {
        var cacheKey = GetCacheWithPrefix(key);
        var cachedValue = _cacheProvider.GetAsync<string>(cacheKey).Result;
        if (cachedValue is null)
            return false;
        _cacheProvider.RemoveAsync(cacheKey).GetAwaiter().GetResult();
        return _cryptoProvider.Decrypt(cachedValue).Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetCacheWithPrefix(key);
        var cachedValue = await _cacheProvider.GetAsync<string>(cacheKey, cancellationToken);
        if (cachedValue is null)
            return false;
        await _cacheProvider.RemoveAsync(cacheKey, cancellationToken);
        return _cryptoProvider.Decrypt(cachedValue).Equals(value, StringComparison.OrdinalIgnoreCase);
    }
}