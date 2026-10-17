# Agent.md

Guidance for AI coding agents (and humans) working in this repository.

## What this is

**Wonderful Captcha** is a .NET 10 library that generates image CAPTCHAs (PNG as a base64 data URI) and verifies user answers against a value stored in a pluggable cache (in-memory, Redis via `IDistributedCache`, EasyCaching, or custom).

NuGet packages produced:

| Project | PackageId |
|---|---|
| `src/Core` | `WonderfulTools.WonderfulCaptcha` |
| `src/Cache.InMemory` | `WonderfulTools.WonderfulCaptcha.Cache.InMemory` |
| `src/Cache.Redis` | `WonderfulTools.WonderfulCaptcha.Cache.Redis` |
| `src/Cache.EasyCaching` | (not packaged yet — no PackageId) |

## Layout

```
Wonderful_Captcha.sln
Agent.md                  <- this file
docs/                     <- all project documentation (start at docs/README.md)
src/Core/                 <- the library
  Dtos/                   options + result records
  Enums/                  public enums used by options
  Extensions/             AddWonderfulCaptcha() DI extension, Helpers (random)
  Implementations/
    WonderfulCaptchaService.cs   Generate / Verify entry point
    Text/                 ITextStrategy implementations + TextFactory
    Image/                CanvasBuilder -> ContentWriter -> FilterEngine pipeline, Utils
    Crypto/               AES encryption of the cached value
  Interfaces/             public extension points
  Fonts/                  ARIAL.TTF, TARSICA.TTF (embedded resources)
src/Cache.*/              cache provider adapters (one ICacheProvider each)
Samples/sample.ApiClient  ASP.NET Core sample using project references
Samples/sample.NugetAlpine sample consuming the published NuGet on Alpine (no OS fonts)
```

See [docs/architecture.md](docs/architecture.md) for the request flow.

## Build & run

```bash
dotnet build Wonderful_Captcha.sln            # GeneratePackageOnBuild=True -> .nupkg in bin/
dotnet run --project Samples/sample.ApiClient # Swagger at /swagger, endpoints /Test/Generate, /Test/Verify
```

SDK: .NET 10. There is **no test project** yet; verify behaviour changes with a small console harness or the sample API.

## Rules that matter (read before changing code)

1. **Never mutate the registered `CaptchaOptions` at runtime.** `IOptions<CaptchaOptions>.Value` is a singleton shared by every request. `GenerateAsync` clones it (`CaptchaOptions.Clone()`) and passes the clone through the image pipeline as a parameter. Any new per-captcha state must live on that clone or in locals — never on `_options`, a static, or a field of a DI service. Breaking this rule reintroduces the "valid code is rejected" race (see [docs/bug-reports/verify-fails-randomly.md](docs/bug-reports/verify-fails-randomly.md)).
2. **When adding a property to any `*Options` class**, keep it a value type, `string`, or `Type` — `Clone()` uses `MemberwiseClone`. If you add a reference-type property (list, nested class), update the corresponding `Clone()` to deep-copy it.
3. **Randomness:** use `Random.Shared` (thread-safe) for visual noise and `RandomNumberGenerator` for the secret text. Never use a shared `new Random()`.
4. **Pipeline components are stateless.** `ICanvasBuilder`, `IContentWriter`, `IFilterEngine`, `IImageGenerator` receive `CaptchaOptions` per call. Keep them free of mutable fields.
5. **Verification is one-shot:** `VerifyAsync` removes the key before comparing. A wrong answer burns the captcha; clients must request a new one.
6. **Comparison is `OrdinalIgnoreCase`.** Text strategies must not emit characters that are only distinguishable by case or that look alike (`I l 1 O o 0`).
7. **Fonts are embedded** (`LogicalName="WonderfulCaptcha.Fonts.<file>"`) so Alpine/containers work without OS fonts. Don't switch to `SystemFonts`.
8. Changing a public interface in `src/Core/Interfaces` is a breaking change for consumers — bump the package `<Version>` accordingly and note it in the docs.

## Conventions

- Namespace root `WonderfulCaptcha` (file-scoped namespaces), sub-namespaces `.Cache`, `.Crypto`, `.Images`, `.Text`.
- `ImplicitUsings` + `Nullable` enabled; global usings in `src/Core/GlobalUsing.cs`.
- Cache providers are registered as **singletons** (`TryAddSingleton`); everything else is scoped.
- Cache provider packages expose one extension `Use<Name>CacheProvider(this CaptchaOptions)` that sets `CacheOptions.CacheProvider`.

## Known issues / backlog

Tracked in [docs/troubleshooting.md](docs/troubleshooting.md#known-issues--backlog).
