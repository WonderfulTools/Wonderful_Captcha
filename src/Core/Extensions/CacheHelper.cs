// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace WonderfulCaptcha;
public static class CacheHelper
{
    private static readonly string AvaCachePrefix = "Ava_";
    public static readonly Dictionary<string, string> Prefixes = new()
    {
        { "Captcha", $"{AvaCachePrefix}Captcha_" },
        { "External", $"{AvaCachePrefix}External_" },
        { "VisitLog", $"{AvaCachePrefix}VisitLog_" },
        { "Public_Content", $"{AvaCachePrefix}Public_Content_" },
        { "Public_Podcast", $"{AvaCachePrefix}Public_Podcast_" },
    };

    public static string GetCaptchaCacheWithPrefix(string key = default!) => Prefixes.GetValueOrDefault("Captcha") + key;
    public static string GetExternalCacheWithPreFix(string key = default!) => Prefixes.GetValueOrDefault("External") + key;
    public static string GetPublicContentCacheWithPreFix(string key = default!) => Prefixes.GetValueOrDefault("Public_Content") + key;
    public static string GetPublicPodcastCacheWithPreFix(string key = default!) => Prefixes.GetValueOrDefault("Public_Podcast") + key;
    public static string GetVisitLogCacheWithPreFix(int refrenceTypeId, Guid refrenceId)
        => Prefixes.GetValueOrDefault("VisitLog") + refrenceTypeId + "_" + refrenceId;

}
