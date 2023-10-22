using WonderfulCaptcha.Images;

namespace WonderfulCaptcha;

public partial class WonderfulCaptchaService : IWonderfulCaptchaService
{
    private CaptchaOptions _options;
    private IImageGenerator _imageGenerator;

    public WonderfulCaptchaService(CaptchaOptions captchaOptions, IImageGenerator imageGenerator)
    {
        _options = captchaOptions;
        _imageGenerator = imageGenerator;
    }

    public string Generate()
    {
        var value = _options.TextProvider.GetInstance(StrategyEnum.Digits)
            .GetText(Helpers.GetRandomNumberBetween(_options.TextLen.Min, _options.TextLen.Max));
        var value2 = _options.TextProvider.GetInstance(StrategyEnum.Character).GetText(10);
        return value + "-" + value2;
    }

    public async Task<CaptchaResult> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();
        _options.Text = _options.TextProvider.GetInstance(_options.Strategy)
            .GetText(Helpers.GetRandomNumberBetween(_options.TextLen.Min, _options.TextLen.Max));
        await _options.CacheProvider.SetAsync(key, _options.CryptoProvider.Encrypt(_options.Text), _options.CacheExpirationTime, cancellationToken);
        var image = await _options.ImageGenerator.GenerateImageAsync(cancellationToken);
        return new CaptchaResult(key, image);
    }

    public bool Verify(string key, string value)
    {
        var cachedValue = _options.CacheProvider.GetAsync<string>(key).Result;
        //_cacheProvider.RemoveAsync(key);
        return _options.CryptoProvider.Decrypt(cachedValue) == value;
    }

    public async Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _options.CacheProvider.GetAsync<string>(key, cancellationToken);
        //await _cacheProvider.RemoveAsync(key, cancellationToken);
        return _options.CryptoProvider.Decrypt(cachedValue).ToLower() == value.ToLower();
    }
}

