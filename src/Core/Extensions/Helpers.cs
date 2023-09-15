namespace WonderfulCaptcha;
public static class Helpers
{
    private static readonly Random _random = new Random();
    public static int GetRandomNumberBetween(int min, int max)
    {
        if (min >= max + 1)
            min = max;
        return _random.Next(min, max + 1);
    }
}

