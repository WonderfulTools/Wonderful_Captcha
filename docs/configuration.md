# Configuration reference

All options live on `CaptchaOptions` and are set in `AddWonderfulCaptcha(o => …)` (global defaults) or in `GenerateAsync(o => …)` (one captcha only).

## TextOptions

| Property | Default | Notes |
|---|---|---|
| `Strategy` | `Character` | See [text-strategies.md](text-strategies.md) |
| `TextLen` | `(5, 5)` | `(Min, Max)` inclusive; ignored by the math strategies |
| `TextColor` | `Random` | `ColorEnum`; ignored when `TextColorHex` is set |
| `TextColorHex` | `null` | e.g. `"#1a73e8"` |
| `TextFont` | `Arial` | `Arial`, `Tarsica`, `Random` (fonts are embedded) |
| `TextFontStyle` | `Random` | `Italic`, `Bold`, `BoldItalic`, `Regular`, `Random` |
| `TextBrush` | `Solid` | Pattern brushes (`Percent20`, `Horizontal`, …) reduce readability |
| `FontSize` | `50` | Base size in px |
| `FontSizeVarietyRange` | `5` | ± random change per character (cumulative) |
| `TextShadow` | `false` | Draws a grey offset copy of each char |
| `TextRotationRange` | `12` | ± degrees per char, `0` disables |
| `TextSkewRange` | `10` | ± degrees per char, `0` disables |
| `Text` / `Value` | — | **Internal, set per captcha.** Do not set them yourself. |

## ImageOptions

| Property | Default | Notes |
|---|---|---|
| `ImageSizeStrategy` | `Dynamic` | `Dynamic`: size computed from text. `Fit`: fixed `ImageSize`, font shrinks to fit. `RelativeFit`: dynamic unless it exceeds `ImageSize` by more than the threshold |
| `ImageSize` | `(350, 100)` | Used by `Fit` / `RelativeFit` |
| `RelativeFitSizeThreshold` | `25` | Used by `RelativeFit` |
| `ImageBackgroundColor` | `White` | Ignored when `ImageBackgroundColorHex` is set |
| `ImageBackgroundColorHex` | `null` | |
| `CharSpacing` | `10` | px between characters |
| `CharPositionVarietyRange` | `(15, 15)` | Random jitter in px (X ±, Y 0..2×) |
| `Complexity` | `0` | Not used yet |

## NoiseOptions

| Property | Default | Notes |
|---|---|---|
| `MaxLineNumbers` | `3` | Random lines in text colour |
| `SaltAndPepperDensityPercent` | `0` | Percent of pixels turned into noise (the parameterless constructor sets 0) |
| `OilPaintLevel` | `0` | `> 0` applies oil-paint filter; expensive on large images |

## CacheOptions

| Property | Default | Notes |
|---|---|---|
| `CacheProvider` | — | **Required.** Set via `UseInMemoryCacheProvider()`, `UseRedisCacheProvider()`, `UseEasyCachingProvider()` or `UseCustomCacheProvider<T>()` |
| `CacheExpirationTime` | `1 minute` | Absolute TTL from generation. 1 minute is short for real users — 2–5 minutes is a common choice |
| `Prefix` | `"WonderfulCaptcha"` | Cache key = `{Prefix}_{guid}`. Set it **only globally**: `Verify` uses the global prefix, so changing it in a per-call `GenerateAsync` override makes that captcha unverifiable |
