using Cache;
using Cache.implementations;
using EasyCaching.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WonderfulCaptcha.Crypto;
using WonderfulCaptcha.Images;
using WonderfulCaptcha.Text;

namespace WonderfulCaptcha;
public static class CaptchaServiceCollectionExtensions
{
    public static void AddWonderfulCaptcha(this IServiceCollection services, IConfiguration configuration, Action<CaptchaOptions>? options = null)
    {
        services.AddTransient<IWonderfulCaptchaService, WonderfulCaptchaService>();
        services.AddTransient<ICryptoEngine, SHA256CryptoEngine>();
        services.AddTransient<ITextFactory, TextFactory>();
        services.AddTransient<IImageGenerator, ImageGenerator>();
        services.AddTransient<IContentWriter, ContentWriter>();
        services.AddTransient<IFilterEngine, FilterEngine>();
        services.AddTransient<ICanvasBuilder, CanvasBuilder>();
        var captchaOptions = new CaptchaOptions();
        options?.Invoke(captchaOptions);
        SetCacheProvider(services, captchaOptions.CacheProvider, configuration);
        services.AddScoped(s => new CaptchaOptions
        {
            Text = captchaOptions.Text,
            CacheExpirationTime = captchaOptions.CacheExpirationTime,
            CacheProvider = captchaOptions.CacheProvider,
            CharPositionVarietyRange = captchaOptions.CharPositionVarietyRange,
            CharSpacing = captchaOptions.CharSpacing,
            Color = captchaOptions.Color,
            FontSize = captchaOptions.FontSize,
            FontSizeVarietyRange = captchaOptions.FontSizeVarietyRange,
            RelativeFitSizeThreshold = captchaOptions.RelativeFitSizeThreshold,
            Size = captchaOptions.Size,
            SizeStrategy = captchaOptions.SizeStrategy,
            Strategy = captchaOptions.Strategy,
            TextBrush = captchaOptions.TextBrush,
            TextColor = captchaOptions.TextColor,
            TextFont = captchaOptions.TextFont,
            TextFontStyle = captchaOptions.TextFontStyle,
            TextLen = captchaOptions.TextLen,
            TextRotationRange = captchaOptions.TextRotationRange,
            TextShadow = captchaOptions.TextShadow,
            TextSkewRange = captchaOptions.TextSkewRange,
            Noise = new NoiseOptions
            {
                Density = captchaOptions.Noise.Density,
            },
        });
    }

    private static void SetCacheProvider(IServiceCollection services, Type? cacheProvider, IConfiguration configuration)
    {
        if (cacheProvider is null)
            cacheProvider = typeof(InMemoryCacheProvider);

        if (cacheProvider == typeof(InMemoryCacheProvider))
        {
            services.AddMemoryCache();
        }
        else if (cacheProvider == typeof(RedisCacheProvider))
        {
            var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                //options.InstanceName = redisSettings.CachePrefix;
            });
        }
        else if (cacheProvider == typeof(EasyCachingProvider))
        {
            var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();
            services.AddEasyCaching(options =>
            {
                options.WithJson("JsonSerializer");
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(redisSettings.Host, redisSettings.Port));
                    config.DBConfig.Password = redisSettings.Password;
                    config.DBConfig.KeyPrefix = redisSettings.CachePrefix;
                    config.SerializerName = "JsonSerializer";
                    config.DBConfig.AllowAdmin = true;
                    config.DBConfig.SyncTimeout = 10000;
                    config.DBConfig.AsyncTimeout = 10000;
                    config.DBConfig.ConnectionTimeout = 10000;
                }, "Wonderful_Redis");
            });
        }
        services.TryAddSingleton(typeof(ICacheProvider), cacheProvider);
    }
}