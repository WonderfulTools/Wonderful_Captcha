
# Wonderful Captcha

Wonderful Captcha is a simple and customizable CAPTCHA (Completely Automated Public Turing test to tell Computers and Humans Apart) implementation for .NET applications.

## Features




- Generate custom CAPTCHA images with random text.
- Easily integrate CAPTCHA functionality into your .NET web forms or applications.
- Customizable CAPTCHA image size, font, text color, background color, and noise level.

# Usage

1- You can install Wonderful Captcha via NuGet Package Manager:

```bash
Install-Package WonderfulCaptcha
```
2- Use extension To Add WonderfulCaptcha
```csharp
using WonderfulCaptcha;

// use extension to add wonderfulcaptcha to project
builder.Services.AddWonderfulCaptcha();
```
3- Generate CAPTCHA image in controler, service, ...


```csharp
[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly IWonderfulCaptchaService _wonderfulCaptchaService;
    public TestController(IWonderfulCaptchaService wonderfulCaptchaService)
    {
        _wonderfulCaptchaService = wonderfulCaptchaService;
    }

    [HttpGet]
    public async Task<ActionResult> GenerateAsync(CancellationToken cancellationToken)
    {
        var value = await _wonderfulCaptchaService.GenerateAsync(cancellationToken: cancellationToken);
        return Ok(value);
    }
}
```
4- Verify CAPTCHA
```csharp
var result = await _wonderfulCaptchaService.VerifyAsync(key, value, cancellationToken);
```

Here's an improved version of your package markdown file:

# Caching

When using WonderfulCaptcha, you have the flexibility to choose from various cache providers to optimize performance and resource management.

## InMemoryCache

The `InMemoryCache` provider is suitable for smaller applications or scenarios where you prefer an in-memory caching mechanism.

### Installation

```bash
Install-Package WonderfulTools.WonderfulCaptcha.Cache.InMemory
```

### Usage

```csharp
builder.Services.AddMemoryCache();
builder.Services.AddWonderfulCaptcha(o =>
{
    o.UseInMemoryCacheProvider();
});
```

## RedisCache

For larger applications or distributed environments, `RedisCache` is an efficient choice due to its scalability and robustness.

### Installation

```bash
Install-Package WonderfulTools.WonderfulCaptcha.Cache.Redis
```

### Usage

```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MyRedisInstance"; 
});
builder.Services.AddWonderfulCaptcha(o =>
{
    o.UseRedisCacheProvider();
});
```

## Custom Cache Provider

If you have specific caching requirements or wish to integrate with a different caching system, you can implement your own cache provider by implementing the `ICacheProvider` interface.

### Usage

1. Implement your custom cache provider:

```csharp
public class MyCacheProvider : ICacheProvider
{
    // Your Implementation Here
}
```

2. Register your custom cache provider:

```csharp
builder.Services.AddWonderfulCaptcha(o =>
{
    o.UseCustomCacheProvider<MyCacheProvider>();
});
```

By providing these options, WonderfulCaptcha ensures compatibility with various caching strategies, allowing you to optimize your application's performance according to your specific needs.

In this version, I've organized the content into clear sections for each cache provider, provided clear installation and usage instructions, and improved the overall readability of the document. Additionally, I've added more descriptive text to explain the benefits and use cases of each cache provider.
