using Cache;
using Cache.implementations;
using Core.Implementations;
using Core.Interfaces;
using EasyCaching.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WonderfulCaptcha;
public static class CaptchaServiceCollectionExtensions
{
    public static void AddWonderfulCaptcha(this IServiceCollection services, IConfiguration configuration, Action<CaptchaOptions>? options = null)
    {
        services.AddSingleton<IWonderfulCaptchaService, WonderfulCaptchaService>();
        var captchaOptions = new CaptchaOptions();
        options?.Invoke(captchaOptions);
        SetCacheProvider(services, captchaOptions.CacheProvider, configuration);
    }

    private static void SetCacheProvider(IServiceCollection services, Type? cacheProvider, IConfiguration configuration)
    {
        if (cacheProvider is null)
            cacheProvider = typeof(RedisCacheProvider);

        //if (captchaProvider is InMemoryCacheProvider)
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
                options.InstanceName = redisSettings.CachePrefix;
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