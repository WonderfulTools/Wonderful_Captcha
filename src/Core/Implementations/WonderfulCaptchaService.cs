using Microsoft.Extensions.Options;
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

    public CaptchaResult Generate(CaptchaOptions options = default!)
    {
        throw new NotImplementedException();
    }

    public async Task<CaptchaResult> GenerateAsync(CaptchaOptions options = default!, CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();

        _options.TextOptions.Text = _textProvider.GetInstance(_options.TextOptions.Strategy)
            .GetText(Helpers.GetRandomNumberBetween(_options.TextOptions.TextLen.Min, _options.TextOptions.TextLen.Max));

        await _cacheProvider.SetAsync(key, _cryptoProvider.Encrypt(_options.TextOptions.Text), _options.CacheOptions.CacheExpirationTime, cancellationToken);

        var image = await _imageGenerator.GenerateImageAsync(cancellationToken);
        return new CaptchaResult(key, image);
    }

    public bool Verify(string key, string value)
    {
        var cachedValue = _cacheProvider.GetAsync<string>(key).Result;
        _cacheProvider.RemoveAsync(key);
        return _cryptoProvider.Decrypt(cachedValue) == value;
    }

    public async Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _cacheProvider.GetAsync<string>(key, cancellationToken);
        await _cacheProvider.RemoveAsync(key, cancellationToken);
        return _cryptoProvider.Decrypt(cachedValue).ToLower() == value.ToLower();
    }
}

