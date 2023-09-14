// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace WonderfulCaptcha;

public class RedisSettings
{
    public string Password { set; get; } = default!;
    public string Host { set; get; } = default!;
    public int Port { set; get; } = default!;
    public string CachePrefix { set; get; } = default!;

    public string ConnectionString => $"{Host}:{Port}" + (string.IsNullOrEmpty(Password) ? "" : $",password={Password}");
}
