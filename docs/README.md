# Wonderful Captcha — Documentation

| Document | What it covers |
|---|---|
| [getting-started.md](getting-started.md) | Install, register, generate, verify — a working setup in five minutes |
| [configuration.md](configuration.md) | Every option in `CaptchaOptions` with defaults |
| [cache-providers.md](cache-providers.md) | InMemory, Redis, EasyCaching, custom providers, multi-instance deployments |
| [text-strategies.md](text-strategies.md) | `Digits`, `Character`, `SumOfTwoNumbersStrategy`, `SimpleMathStrategy` |
| [architecture.md](architecture.md) | Components, DI lifetimes, generate/verify flow, thread-safety model |
| [extending.md](extending.md) | Custom cache provider, crypto, text strategy, image pipeline |
| [troubleshooting.md](troubleshooting.md) | Common problems and the known-issues backlog |
| [bug-reports/verify-fails-randomly.md](bug-reports/verify-fails-randomly.md) | Root-cause analysis of "a correct code is rejected until the 7th–8th try" |
| [migrations/imagesharp-to-skiasharp.md](migrations/imagesharp-to-skiasharp.md) | Why and how the renderer moved from ImageSharp to SkiaSharp, API changes, verification |

For AI agents and contributors, see [../Agent.md](../Agent.md).
