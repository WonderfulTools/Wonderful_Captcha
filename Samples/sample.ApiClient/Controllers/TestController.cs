using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace sample.ApiClient.Controllers
{
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
            var value = await _wonderfulCaptchaService.GenerateAsync(cancellationToken);
            return Ok(value);
        }

        [HttpGet]
        public async Task<ActionResult> VerifyAsync(string key, string value, CancellationToken cancellationToken)
        {
            var result = await _wonderfulCaptchaService.VerifyAsync(key, value, cancellationToken);
            return Ok(result);
        }
    }
}