using Core.Interfaces;

namespace Core.Implementations;
public class WonderfulCaptchaService : IWonderfulCaptchaService
{
    public async Task<string> GenerateCaptchaAsync(CancellationToken cancellationToken)
        => "iVBORw0KGgoAAAANSUhEUgAAAJYAAAAyCAYAAAC+jCIaAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcE" +
        "hZcwAADsMAAA7DAcdvqGQAAANOSURBVHhe7ZaBkdwwCEW3l3SRIlJDSkgJ6TxZZqPogwHJsnTWeP6bYe52BfiDkLwvQgghhB" +
        "BCCCGEEEIIIYRsxp/Xr7f9/mff39biG/iLSbzkydHP+fnfX3/fa7nOsZzWdE176NSaPHTOnv1chBYiQ5NzbEYdkhgpsPhLfGmQH" +
        "dKz5jX6ak6xH29DdtKZ9RtzYp9vQIsWYRk4IMXag6X98RmygeV7u5kedjO8Z5/N2cPdOm3fI/+zOheiBecTXv3w1moVEBd79r" +
        "b8gPmOp3IsZ84eOqNbv7Ki9kHk4UWI2FFsRU4dii7/Z4OV5cc1v1E+9vTOyJmxi85WzIrah+kVg02S/zFOPkfgMFq/+CbLQS1" +
        "W82jOjF10Yj4vZlTnEvoG67OGPlikHZgC+hx/Y9ScYqKjl7iB4zlj9tFp3xiaFbVfIN/8D9igIrg9WLpQ+YxInrImfnY9Jm7g" +
        "eM6YXXRijLdPK2q/BA6IPlUfUDCue8OG4LqXt7UegXptA0dzZuyhM85VGNW5jNZg4fUbNec4WHXNb8Tn+zg+osYc40ZzZtyvE/v" +
        "v91JYUftFcEDsKw2Hzq5hnC02/y3QvtZ9aoyY1YM5R6z1erlbp9fHAuaMh++LiQerfu81FYcHC+nZjGyYPfBZUQzmHDFv43bS" +
        "mQ3MWZ1fAjYDm4tivab7g6WvZL8R2ueMxc3tee5Z7teJhzTOu6L2CXiDhQXJgB2pxWCxclpKXHRybLN6rHUKMafUM4N9dOpnSA" +
        "5kRe1TON48/tBoPJ+aJ45bc20/O2fttRe3QucEjgMi4opQezoqxzgssCdOTD5f5+k5a4yY7u0KnVOowuQvFpFfq9oPr2MZsI" +
        "gV1/bTc2KcGA7PCp2T0MLEymcs4IgfV4YzYp/XS85OOXFP7KFdoXMSVRhaW2T1xas4j1txbT87Z3ZbrdE5DS1cTMS2RNqCxeR" +
        "kZay4tp+c0/ZYPiMrdE7DihezBXh4ca1hlOKL76xr+5k5q28xb09W6JyGHRD7Do+QQs7Faf/WEPahc46a3hS9drdOGR4f37/Hevf" +
        "4EnZAehuJcT2vTvyRGTfrHJhz1Kz2XXS2bqArtffuMSGEEEIIIYQQQgghhBBCCCGEEEIIIYQQQnbl9foLnXoL/NeU8ycAAAAASUV" +
        "ORK5CYII=";
}

