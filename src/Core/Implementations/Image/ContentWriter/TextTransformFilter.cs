using System.Numerics;

namespace WonderfulCaptcha.Images;

public static class TextTransformFilter
{
    private static readonly Random Random = new Random();
    public static Matrix3x2 ApplyTransforms(PointF position, CaptchaOptions options)
    {
        var transform = 
            Skew(position, options) *
            Rotation(position, options);
        
        return transform;
    }
    
    private static Matrix3x2 Rotation(PointF position, CaptchaOptions options)
    {
        if (options.TextRotationRange == 0)
            return Matrix3x2.Identity;

        var rotation = (float)Random.Next(-options.TextRotationRange, options.TextRotationRange);
        return Matrix3x2.CreateRotation(rotation.ToRadian(), position);
    }
    
    private static Matrix3x2 Skew(PointF position, CaptchaOptions options)
    {
        if (options.TextSkewRange == 0)
            return Matrix3x2.Identity;

        var xSkew = ((float)Random.Next(-options.TextSkewRange, options.TextSkewRange)).ToRadian();
        var ySkew = ((float)Random.Next(-options.TextSkewRange, options.TextSkewRange)).ToRadian();

        return Matrix3x2.CreateSkew(xSkew, ySkew, position);
    }
    
    
    
}