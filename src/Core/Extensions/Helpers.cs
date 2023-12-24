namespace WonderfulCaptcha;
public static class Helpers
{
    private static readonly Random Random = new Random();
    public static int GetRandomNumberBetween(int min, int max)
    {
        if (min >= max + 1)
            min = max;
        return Random.Next(min, max + 1);
    }
}

