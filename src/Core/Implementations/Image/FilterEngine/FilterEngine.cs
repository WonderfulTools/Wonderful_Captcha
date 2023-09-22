namespace WonderfulCaptcha.Images;

public class FilterEngine : IFilterEngine
{
    private readonly CaptchaOptions _captchaOptions;

    public FilterEngine(CaptchaOptions captchaOptions)
        => _captchaOptions = captchaOptions;
    

    public void ApplyFilters(Image<Rgba32> image)
    {
        image.Mutate(x => x.GaussianBlur(0.5f)
            .Saturate(0.8f));

        var random = new Random();
        for (int x = 0; x < image.Width; x++)
        for (int y = 0; y < image.Height; y++)
            if (random.NextDouble() < _captchaOptions.Noise.Density)
                image[x, y] = random.Next(2) == 0 ? ColorUtils.GetColor(_captchaOptions.Color) : Color.White;

    }
}