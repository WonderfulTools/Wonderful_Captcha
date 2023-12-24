using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WonderfulCaptcha.Cache.InMemory;
public static class CaptchaOptionsExtensions
{
    public static void UseInMemoryCacheProvider(this IWonderfulCaptchaBuilder _, IServiceCollection services)
          => services.TryAddScoped<ICacheProvider, InMemoryCacheProvider>();
}
