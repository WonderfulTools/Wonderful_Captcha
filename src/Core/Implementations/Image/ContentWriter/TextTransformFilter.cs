using System.Numerics;

namespace WonderfulCaptcha.Images;

public static class TextTransformFilter
{
    private static readonly Random Random = Random.Shared;
    public static SKMatrix ApplyTransforms(SKPoint position, CaptchaOptions options)
    {
        var origin = new Vector2(position.X, position.Y);
        var transform =
            Skew(origin, options) *
            Rotation(origin, options);

        return ToSKMatrix(transform);
    }

    // System.Numerics uses row vectors (p' = p * M); SKMatrix uses column vectors (p' = M * p).
    private static SKMatrix ToSKMatrix(Matrix3x2 m)
        => new(m.M11, m.M21, m.M31,
               m.M12, m.M22, m.M32,
               0, 0, 1);

    private static Matrix3x2 Rotation(Vector2 position, CaptchaOptions options)
    {
        if (options.TextOptions.TextRotationRange == 0)
            return Matrix3x2.Identity;

        var rotation = (float)Random.Next(-options.TextOptions.TextRotationRange, options.TextOptions.TextRotationRange);
        return Matrix3x2.CreateRotation(rotation.ToRadian(), position);
    }

    private static Matrix3x2 Skew(Vector2 position, CaptchaOptions options)
    {
        if (options.TextOptions.TextSkewRange == 0)
            return Matrix3x2.Identity;

        var xSkew = ((float)Random.Next(-options.TextOptions.TextSkewRange, options.TextOptions.TextSkewRange)).ToRadian();
        var ySkew = ((float)Random.Next(-options.TextOptions.TextSkewRange, options.TextOptions.TextSkewRange)).ToRadian();

        return Matrix3x2.CreateSkew(xSkew, ySkew, position);
    }



}