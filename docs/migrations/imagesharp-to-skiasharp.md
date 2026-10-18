# Migration: SixLabors.ImageSharp → SkiaSharp

| | |
|---|---|
| Date | 2026-10-17 |
| Removed | `SixLabors.ImageSharp` 3.1.12, `SixLabors.ImageSharp.Drawing` 2.1.7 |
| Added | `SkiaSharp` 4.153.1, `SkiaSharp.NativeAssets.Linux.NoDependencies` 4.153.1 (both MIT) |

## Why

- Five advisories (3 high, 2 moderate, published 2026-10-07) affect every ImageSharp version up to 4.1.1; the only patched release is **4.1.2**. There is no 3.x fix.
- ImageSharp 4.x enforces a **build-time license key** for direct package references (Release builds fail without `sixlabors.lic` / `SixLaborsLicenseKey`). Commercial use above 1M USD revenue requires a paid license.
- The advisories are in the TIFF encoders/decoder, BigTIFF EXIF parsing, ICC profile parsing and `HistogramEqualization` — code paths WonderfulCaptcha never used (it only creates a blank image, draws and encodes PNG). So the practical risk was low, but every consumer still got `NU1903` warnings, and `TreatWarningsAsErrors`/audit gates fail the build.

SkiaSharp is MIT-licensed, has no license key, is maintained by Microsoft/Uno Platform, and is ~4× faster here (≈1.5 ms vs ≈6 ms per default captcha).

## What did NOT change

- `IWonderfulCaptchaService` (`GenerateAsync`, `VerifyAsync`, `Verify`), `CaptchaResult(Key, Image)`.
- Output format: `data:image/png;base64,…` PNG.
- All `CaptchaOptions` properties, enums, defaults, and the cache providers.
- **Image dimensions are identical** for every size strategy (character width/height are measured from exact glyph outlines; baseline uses the same `hhea`-based layout as ImageSharp). Verified: 257×65, 213×65, 324×80, 350×100, 344×65 in both engines.
- Embedded fonts (no OS fonts; Alpine works). Verified on `mcr.microsoft.com/dotnet/aspnet:10.0-alpine` with no `/usr/share/fonts`.
- Behaviour of `FontStyleEnum`: the embedded fonts only contain a regular face, so — as before — glyphs are drawn regular.

Pixels are not byte-identical (different rasterizer/antialiasing); the look is the same.

## Breaking changes (only for code that extends the image pipeline)

| Before (ImageSharp) | After (SkiaSharp) |
|---|---|
| `ICanvasBuilder.Create(CaptchaOptions)` → `Image<Rgba32>` | → `SKBitmap` |
| `IContentWriter.WriteText(Image<Rgba32>, CaptchaOptions)` | `WriteText(SKBitmap, CaptchaOptions)` |
| `IFilterEngine.ApplyFilters(Image<Rgba32>, CaptchaOptions)` | `ApplyFilters(SKBitmap, CaptchaOptions)` |
| `ColorUtils.GetColor(...)` → `Color` | → `SKColor` |
| `FontUtils.GetFont(...)` → `Font` | → `SKFont` (disposable) |
| `FontUtils.GetFontFamily(...)` → `FontFamily` | → `SKTypeface` |
| `FontUtils.GetFontStyle(...)` → `FontStyle` | → `SKFontStyle` |
| `FontUtils.GetCharWidth(Font)` | `GetCharWidth(SKFont)` |
| `BrushUtils.GetBrush(BrushEnum, Color)` → `Brush` | `GetBrush(BrushEnum, SKColor, SKMatrix? canvasTransform = null)` → `SKPaint` (disposable) |
| `TextTransformFilter.ApplyTransforms(PointF, …)` → `Matrix3x2` | `ApplyTransforms(SKPoint, …)` → `SKMatrix` |
| `FontSettings.DefaultFontStyle` (`const FontStyle`) | `static readonly SKFontStyle` |

New helpers: `FontUtils.MeasureSize(string, SKFont)`, `FontUtils.GetBaselineOffset(SKFont)`.

`ColorUtils.GetColor(string hex)` keeps ImageSharp's format: `#rgb`, `#rgba`, `#rrggbb`, `#rrggbbaa` (alpha **last**), unlike `SKColor.Parse` (alpha first).

## Feature mapping

| Feature | ImageSharp | SkiaSharp implementation |
|---|---|---|
| Canvas | `new Image<Rgba32>(w, h, bg)` | `SKBitmap` RGBA8888 premul + `Erase(bg)` |
| Font loading | `FontCollection.Add(stream)` | `SKTypeface.FromData(SKData.Create(stream))`, cached |
| Glyph size | `TextMeasurer.MeasureSize` | `SKFont.GetTextPath(...).TightBounds` |
| Text position | `DrawText(options, text, font, brush, topLeft)` | `canvas.DrawText(text, x, y + baselineOffset, font, paint)` |
| Rotation + skew | `DrawingOptions.Transform` (`Matrix3x2`) | same `Matrix3x2` converted to `SKMatrix`, `canvas.SetMatrix` |
| Hatch brushes | `Brushes.Percent20/…` | identical 4×4 patterns as a repeating nearest-neighbour image shader, kept aligned to the pixel grid |
| Lines | `Pens.Solid` + `DrawLine` | `SKPaint` stroke + `DrawLine` |
| Salt & pepper | `image[x, y] = color` | `SetPixel` |
| Oil paint | `OilPaint(levels, 8)` | managed mode filter, same window/levels semantics |
| Blur + saturate | `GaussianBlur(0.5f).Saturate(0.8f)` | `SKImageFilter.CreateBlur(0.5, clamp)` + saturation colour matrix |
| PNG | `SaveAsPngAsync` | `SKBitmap.Encode(Png)` |

## Verification

- Solution builds without ImageSharp warnings.
- Visual comparison of 9 configurations (default, digits, math + oil paint, shadow + hex colours, diagonal and percent20 brushes, salt & pepper, Fit + Tarsica, RelativeFit).
- Concurrency generate → verify with real InMemory and Redis providers: 0 failures at 10/100/500 concurrent requests.
- `Samples/sample.NugetAlpine` built against a locally packed package and run on Alpine without OS fonts: HTTP 200, valid PNGs.

## Release

Ship together with the shared-options fix as **11.0.0** (both change the image-pipeline interfaces).
