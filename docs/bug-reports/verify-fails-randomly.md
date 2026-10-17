# Bug: correct captcha is rejected; succeeds randomly after 7–8 attempts

| | |
|---|---|
| Affected | `WonderfulTools.WonderfulCaptcha` ≤ 10.3.0, all cache providers |
| Severity | High — legitimate users are blocked |
| Status | Fixed (pending release) |
| Date | 2026-10-17 |

## Symptom

A user requests a captcha, types exactly what the image shows, and `VerifyAsync` returns `false`. After 7–8 new captchas one is accepted "at random". It happens more often under load and with Redis.

## Hypotheses checked

| Hypothesis | Verdict |
|---|---|
| Code is not thread-safe; concurrent generate requests collide | **Confirmed — root cause** |
| Redis TTL is too short, key expires before verify | Ruled out as the cause. TTL is applied correctly (`AbsoluteExpirationRelativeToNow = 1 min`); it only matters if the user takes > 60 s. Still worth raising. |
| Encryption/decryption round-trip broken | Ruled out — `Decrypt(Encrypt(x)) == x` for all strategies. |
| Redis JSON (de)serialization changes the value | Ruled out — a JSON string round-trips unchanged. |
| Key/prefix mismatch | Not the cause with default usage; possible only if `Prefix` is changed in a per-call override (documented). |
| Look-alike characters (`Character` strategy) | **Contributing factor** — fixed as well. |

## Root cause

`IOptions<CaptchaOptions>.Value` is a **singleton**: every scope, every request, every service gets the *same* object. The code used it as per-request scratch space:

```csharp
// WonderfulCaptchaService.GenerateAsync (before)
_options.TextOptions.Text  = textResult.Text;     // write into the shared singleton
_options.TextOptions.Value = textResult.Value;
await _cacheProvider.SetAsync(key, Encrypt(_options.TextOptions.Value), …);   // ← await: other requests run here
var image = await _imageGenerator.GenerateImageAsync(…);
// ContentWriter (before) — reads the text back from the same singleton:
var text = _captchaOptions.TextOptions.Text;
```

Timeline with two concurrent requests A and B:

```
A: Text = "48213"  → cache[A] = "48213" → await SetAsync (Redis round-trip) …
B: Text = "90577"  → cache[B] = "90577" → await …
A: draws options.Text  = "90577"   ✗ image A shows B's code
B: draws options.Text  = "90577"   ✓
```

User A types `90577`; the cache for key A holds `48213` → rejected. A retry succeeds only when no other request overlaps the window, which is why it works "randomly" after several tries. The slower the cache (Redis over the network), the wider the window.

Secondary effects of the same design:
- `GenerateAsync(o => …)` per-call overrides permanently changed global options for all later requests.
- `CanvasBuilder` wrote the computed `ImageSize` into the shared options, so concurrent captchas could be drawn on a canvas sized for different text.
- Several helpers used `static readonly Random = new()`. `System.Random` is not thread-safe; under concurrency it can be corrupted and return `0` forever.

## Reproduction

A console harness generated N captchas in parallel against a fake cache with a 5 ms delay (a stand-in for a Redis round-trip), recorded the text actually drawn into each image, and verified with that text:

| Concurrent requests | Before fix: failures | After fix: failures |
|---|---|---|
| 1 | 0 | 0 |
| 10 | 10 | 0 |
| 200 | 199 | 0 |
| 1000 | — | 0 |

## Fix

1. `CaptchaOptions.Clone()` (deep copy) added. `GenerateAsync` clones the global options per call, applies per-call overrides to the clone, stores `Text`/`Value` on the clone only.
2. The clone is passed explicitly through the pipeline. **Interface change** (breaking for custom implementations):
   - `IImageGenerator.GenerateImageAsync(CaptchaOptions options, CancellationToken ct)`
   - `ICanvasBuilder.Create(CaptchaOptions options)`
   - `IContentWriter.WriteText(Image<Rgba32> image, CaptchaOptions options)`
   - `IFilterEngine.ApplyFilters(Image<Rgba32> image, CaptchaOptions options)`

   `CanvasBuilder`, `ContentWriter`, `FilterEngine` no longer depend on `IOptions<CaptchaOptions>` and are stateless.
3. All shared `Random` instances → `Random.Shared`.
4. `TextCharacterStrategy` now uses `RandomNumberGenerator.GetString` over an alphabet without `I l 1 O o 0`. This also removes leaked `=` padding and a possible `ArgumentOutOfRangeException`.

Related fixes in the same change:
- `AddWonderfulCaptcha()` with no arguments threw `NullReferenceException`; it now fails with a clear message if no cache provider is configured.
- `Verify` (sync) did not wait for the key removal.
- The PNG data URI was built from `MemoryStream.GetBuffer()`, which appended unused buffer bytes; now `ToArray()`.
- The `Image` is disposed after encoding.

## Things to check in the consuming app (not library bugs)

- After a failed verify, **fetch a new captcha**: the key is deleted on the first attempt.
- InMemory cache with more than one instance behind a load balancer will reproduce the same symptom — use Redis.
- Consider `CacheExpirationTime = TimeSpan.FromMinutes(2..5)`.

## Release note

Bump the major or minor version (proposed **11.0.0**, since public interfaces changed), and republish `Cache.InMemory` / `Cache.Redis` against the new Core.
