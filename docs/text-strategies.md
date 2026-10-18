# Text strategies

Selected by `TextOptions.Strategy`. Each strategy returns a pair: `Text` (drawn on the image) and `Value` (the expected answer).

| `StrategyEnum` | Image shows | User types | Uses `TextLen` |
|---|---|---|---|
| `Digits` | `30417` | `30417` | yes |
| `Character` (default) | `xV3Qe` | `xv3qe` (case-insensitive) | yes |
| `SumOfTwoNumbersStrategy` | `13+2` | `15` | no |
| `SimpleMathStrategy` | `13+2` or `14-6` | `15` / `8` | no |

## Character alphabet

`Character` draws from a cryptographically secure RNG over:

```
ABCDEFGHJKLMNPQRSTUVWXYZ abcdefghijkmnpqrstuvwxyz 23456789
```

`I`, `l`, `1`, `O`, `o`, `0` are excluded on purpose: once rotated and skewed they are indistinguishable, and case-insensitive matching does not help (`l` ≠ `i`). Earlier versions used the Base64 alphabet, which also leaked `=` padding and could throw for some lengths.

## Custom strategy

`ITextStrategy` instances are created by `TextFactory` (`ITextProvider`). To add one, implement `ITextStrategy` and register your own `ITextProvider` — see [extending.md](extending.md#custom-text-strategy).
