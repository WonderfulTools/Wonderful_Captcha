# Wonderful Captcha

Wonderful Captcha is a simple and customizable CAPTCHA (Completely Automated Public Turing test to tell Computers and Humans Apart) implementation for .NET applications.

## Features

- Generate custom CAPTCHA images with random text.
- Easily integrate CAPTCHA functionality into your .NET web forms or applications.
- Customizable CAPTCHA image size, font, text color, background color, and noise level.

## Usage

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

