using Cache;
using Microsoft.Extensions.Options;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;
public class WonderfulCaptchaService : IWonderfulCaptchaService
{
    private readonly CaptchaOptions captchaOptions;

    private readonly ICacheProvider _cacheProvider;
    private readonly ICryptoEngine _cryptoEngine;
    private readonly ITextFactory _textFactory;
    private readonly IImageGenerator _imageGenerator;
    public WonderfulCaptchaService(ICacheProvider cacheProvider,
                                   ICryptoEngine cryptoEngine,
                                   ITextFactory textFactory,
                                   IOptions<CaptchaOptions> options,
                                   IImageGenerator imageGenerator)
    {
        _cacheProvider = cacheProvider;
        _cryptoEngine = cryptoEngine;
        _textFactory = textFactory;
        captchaOptions = options.Value;
        _imageGenerator = imageGenerator;
    }

    public string Generate()
    {
        var value = _textFactory.GetInstance(StrategyEnum.Digits)
            .GetText(Helpers.GetRandomNumberBetween(captchaOptions.TextLen.Min, captchaOptions.TextLen.Max));
        var value2 = _textFactory.GetInstance(StrategyEnum.Character).GetText(10);
        return value + "-" + value2;
    }

    public async Task<CaptchaResult> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();
        captchaOptions.Text = _textFactory.GetInstance(captchaOptions.Strategy)
            .GetText(Helpers.GetRandomNumberBetween(captchaOptions.TextLen.Min, captchaOptions.TextLen.Max));
        await _cacheProvider.SetAsync(key, _cryptoEngine.Encrypt(captchaOptions.Text), captchaOptions.CacheExpirationTime, cancellationToken);
        var image = await _imageGenerator.GenerateImageAsync(cancellationToken);
        return new CaptchaResult(key, image);
    }

    public async Task<CaptchaResult> GenerateDigitsAsync(CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();
        captchaOptions.Text = _textFactory.GetInstance(StrategyEnum.Digits)
            .GetText(Helpers.GetRandomNumberBetween(captchaOptions.TextLen.Min, captchaOptions.TextLen.Max));
        await _cacheProvider.SetAsync(key, _cryptoEngine.Encrypt(captchaOptions.Text), captchaOptions.CacheExpirationTime, cancellationToken);
        var image = await _imageGenerator.GenerateImageAsync(cancellationToken);
        return new CaptchaResult(key, image);
    }

    public async Task<CaptchaResult> GenerateLettersAsync(CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();
        captchaOptions.Text = _textFactory.GetInstance(StrategyEnum.Character)
            .GetText(Helpers.GetRandomNumberBetween(captchaOptions.TextLen.Min, captchaOptions.TextLen.Max));
        await _cacheProvider.SetAsync(key, _cryptoEngine.Encrypt(captchaOptions.Text), captchaOptions.CacheExpirationTime, cancellationToken);
        var image = await _imageGenerator.GenerateImageAsync(cancellationToken);
        return new CaptchaResult(key, image);
    }

    public bool Verify(string key, string value)
    {
        var cachedValue = _cacheProvider.GetAsync<string>(key).Result;
        //_cacheProvider.RemoveAsync(key);
        return _cryptoEngine.Decrypt(cachedValue) == value;
    }

    public async Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _cacheProvider.GetAsync<string>(key, cancellationToken);
        //await _cacheProvider.RemoveAsync(key, cancellationToken);
        return _cryptoEngine.Decrypt(cachedValue) == value;
    }

    public IWonderfulCaptchaService WithBackGroundColor(string text)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithCaptchaText(string text)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithColor(string text)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithComplexity(StrategyEnum complexity)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithLen(int len)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithSize(int height = 10, int width = 20)
    {
        throw new NotImplementedException();
    }

    public IWonderfulCaptchaService WithStrategy(StrategyEnum strategy)
    {
        throw new NotImplementedException();
    }
}

