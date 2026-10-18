# Architecture

## Components

| Interface | Default implementation | DI lifetime | Role |
|---|---|---|---|
| `IWonderfulCaptchaService` | `WonderfulCaptchaService` | Scoped | Public entry point: `GenerateAsync`, `VerifyAsync`, `Verify` |
| `ITextProvider` | `TextFactory` | Scoped | Returns the `ITextStrategy` for a `StrategyEnum` |
| `ITextStrategy` | `TextDigitStrategy`, `TextCharacterStrategy`, `SumOfTwoNumbersStrategy`, `SimpleMathStrategy` | created by factory | Produces `(Text, Value)` |
| `ICryptoProvider` | `SHA256CryptoEngine` | Scoped | AES-CFB encrypts the value before caching (key = SHA-256 of a constant) |
| `ICacheProvider` | chosen via `CacheOptions.CacheProvider` | **Singleton** | Stores/reads/removes the encrypted value |
| `IImageGenerator` | `ImageGenerator` | Scoped | Runs the image pipeline and encodes PNG → data URI |
| `ICanvasBuilder` | `CanvasBuilder` | Scoped | Computes size, creates the background image |
| `IContentWriter` | `ContentWriter` | Scoped | Draws each character with font/size/position/rotation/skew jitter |
| `IFilterEngine` | `FilterEngine` | Scoped | Lines, salt-and-pepper, oil paint, blur + saturate |
| `IOptions<CaptchaOptions>` | Options framework | **Singleton** | Global defaults — read-only at runtime |

## Generate flow

```
GenerateAsync(configure?)
 ├─ key = Guid.NewGuid()
 ├─ callOptions = _options.Clone()          ← per-call copy of the singleton defaults
 ├─ configure?.Invoke(callOptions)          ← per-call overrides affect this captcha only
 ├─ (Text, Value) = TextFactory.GetInstance(strategy).GetText(len)
 ├─ cache.SetAsync("{Prefix}_{key}", Encrypt(Value), ttl)
 └─ ImageGenerator.GenerateImageAsync(callOptions)
       ├─ CanvasBuilder.Create(callOptions)        (writes computed ImageSize into callOptions)
       ├─ ContentWriter.WriteText(image, callOptions)
       ├─ FilterEngine.ApplyFilters(image, callOptions)
       └─ PNG → "data:image/png;base64,…"
 → CaptchaResult(key, image)
```

## Verify flow

```
VerifyAsync(key, input)
 ├─ cached = cache.GetAsync("{Prefix}_{key}")     ← global Prefix
 ├─ null → false  (expired / unknown / already used / other instance's in-memory cache)
 ├─ cache.RemoveAsync(...)                         ← one-shot, before comparing
 └─ Decrypt(cached).Equals(input, OrdinalIgnoreCase)
```

## Thread-safety model

- The registered `CaptchaOptions` is a process-wide singleton and is **never mutated** after startup.
- All per-captcha state (`Text`, `Value`, computed `ImageSize`, per-call overrides) lives on the cloned `callOptions`, which is passed explicitly down the pipeline. Pipeline components hold no mutable state.
- Randomness uses `Random.Shared` (thread-safe); the secret text in `Character` mode uses `RandomNumberGenerator`.
- Fonts are loaded once into a `ConcurrentDictionary` in `FontUtils`.

Before v-next the pipeline read `IOptions<CaptchaOptions>.Value` directly and `GenerateAsync` wrote `Text`/`Value` into it — see [bug-reports/verify-fails-randomly.md](bug-reports/verify-fails-randomly.md).

## Rendering engine

Images are rendered with **SkiaSharp** (MIT). It replaced SixLabors.ImageSharp / ImageSharp.Drawing in v-next — see [migrations/imagesharp-to-skiasharp.md](migrations/imagesharp-to-skiasharp.md).

- Native binaries: Windows/macOS come with the `SkiaSharp` package; Linux (glibc **and** Alpine/musl) comes from `SkiaSharp.NativeAssets.Linux.NoDependencies`, which has no fontconfig/freetype system dependency.
- Text layout keeps the previous conventions: a draw position is the top-left of a one-em line box, the baseline is placed using the font's `hhea` ascender/descender (`FontUtils.GetBaselineOffset`), and character size is measured from the exact glyph outline (`FontUtils.MeasureSize`). This keeps image sizes identical to the ImageSharp era.
- Filters: lines (`DrawLine`), salt & pepper (`SetPixel`), oil paint (managed mode filter in `FilterEngine`), then blur σ=0.5 + saturation 0.8 via Skia image/colour filters.

## Packaging

`GeneratePackageOnBuild=True` on Core, Cache.InMemory and Cache.Redis: every build writes `.nupkg` files to `bin/<Configuration>/`. Fonts are embedded with `LogicalName="WonderfulCaptcha.Fonts.<file>"` and loaded with `SKTypeface.FromData`, so no OS fonts are needed (Alpine-safe).
