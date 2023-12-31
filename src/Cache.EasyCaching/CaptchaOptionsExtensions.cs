﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WonderfulCaptcha.Cache.EasyCaching;

namespace WonderfulCaptcha.Cache.InMemory;
public static class CaptchaOptionsExtensions
{
    public static void UseInMemoryCacheProvider(this IWonderfulCaptchaBuilder builder, IServiceCollection services)
        => services.TryAddScoped<ICacheProvider, EasyCachingProvider>();
}
