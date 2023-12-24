// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace WonderfulCaptcha;

public class NoiseOptions
{
    public NoiseOptions()
    {
        SaltAndPepperDensityPercent = 0;
    }
    public NoiseOptions(double saltAndPepperDensity = 0.02, int MaxLineNumbers = 3, int OilPaintLevel = 8)
    {
        SaltAndPepperDensityPercent = saltAndPepperDensity;
    }
    public double SaltAndPepperDensityPercent { set; get; } = 2;
    public int MaxLineNumbers { set; get; } = 3;
    public int OilPaintLevel { set; get; } = 0;
}
