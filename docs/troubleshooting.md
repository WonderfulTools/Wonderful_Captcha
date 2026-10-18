# Troubleshooting

## A correct code is rejected, then accepted after several tries

Check, in this order:

1. **Library version.** Versions up to 10.3.0 have a race condition: concurrent `GenerateAsync` calls overwrite each other's text, so the image shows one code while the cache holds another. Fixed in the next release. Full analysis: [bug-reports/verify-fails-randomly.md](bug-reports/verify-fails-randomly.md).
2. **Retrying with the same key.** Verification is one-shot — the first attempt deletes the key. If the UI keeps showing the old image after a failed attempt, every retry returns `false`. Always fetch a new captcha after a failed verify.
3. **Multiple instances + InMemory cache.** Success only when generate and verify land on the same instance. Use Redis, or sticky sessions.
4. **Expiry.** Default TTL is 1 minute from generation. Raise `CacheOptions.CacheExpirationTime` if users are slow.
5. **Prefix mismatch.** `Prefix` changed in a per-call `GenerateAsync(o => …)` override, or different `Prefix`/Redis `InstanceName` across instances.
6. **Look-alike characters** (`Character` strategy before the fix): `I/l/1`, `O/0`.
7. **Front-end sends the wrong key**, e.g. two parallel generate calls (React StrictMode double effects, image refresh on focus) where the form keeps the first key but displays the second image.

Quick Redis check right after generating: `redis-cli --scan --pattern '*WonderfulCaptcha_*'`, then `redis-cli TTL <key>`.

## `InvalidOperationException: No cache provider configured`

Call one of `UseInMemoryCacheProvider()`, `UseRedisCacheProvider()`, `UseEasyCachingProvider()`, `UseCustomCacheProvider<T>()` inside `AddWonderfulCaptcha(o => …)`.

## `Embedded font resource 'WonderfulCaptcha.Fonts.X' not found`

The package was built without the `EmbeddedResource` entry in `Core.csproj`. Use ≥ 10.3.0.

## `DllNotFoundException: libSkiaSharp` / `TypeInitializationException` in SkiaSharp

The native Skia library for the current OS/architecture was not deployed.

- Linux (glibc and Alpine/musl, x64/arm64/arm): provided by `SkiaSharp.NativeAssets.Linux.NoDependencies`, which Core references — make sure a trimmed/`--no-dependencies` publish did not drop it, and that the `runtimes/<rid>/native` folder exists in the output.
- Windows/macOS: provided by `SkiaSharp` itself.
- Self-contained publish: publish with the correct `-r <rid>` (e.g. `linux-musl-x64` for Alpine).

## Unable to resolve `IMemoryCache` / `IDistributedCache`

Add `builder.Services.AddMemoryCache()` or `AddStackExchangeRedisCache(...)` before building the app.

## Known issues / backlog

| Area | Issue |
|---|---|
| Security | `SHA256CryptoEngine` uses a hard-coded key and a zero IV (deterministic ciphertext). Low impact since the value never leaves the server, but the key should be configurable. The class name is misleading (it is AES). |
| API | `Verify` (sync) blocks on async calls (`.Result`). Prefer `VerifyAsync`. |
| API | `NoiseOptions(double, int, int)` constructor ignores `MaxLineNumbers` and `OilPaintLevel` parameters. |
| API | `ImageOptions.Complexity` is unused. |
| Code | `CacheHelper` is unused leftover code. |
| Build | `src/Cache.Redis/Cache - Backup.Redis.csproj` is a stale net7 project file in the same folder as `Cache.Redis.csproj`; running `dotnet build` inside that folder fails with "multiple project files". Delete it. |
| Docs | Root `README.md` contains leftover text ("Here's an improved version of your package markdown file…"). |
| Tests | No automated tests. A concurrency test for generate/verify should be added. |
