using Cache;
using Core.Enums;
using Core.Interfaces;

namespace Core.Implementations;
public class WonderfulCaptchaService : IWonderfulCaptchaService
{
    private readonly ICacheProvider _cacheProvider;
    public WonderfulCaptchaService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public string Generate()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid().ToString();
        var value = "a";
        await _cacheProvider.SetAsync(key, value, TimeSpan.FromMinutes(5), cancellationToken);
        return key;
    }

    public bool Verify(string key, string value)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> VerifyAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var cached = await _cacheProvider.GetAsync<string>(key, cancellationToken);
        return cached == value;
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

