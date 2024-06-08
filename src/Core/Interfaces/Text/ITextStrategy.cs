namespace WonderfulCaptcha.Text;

/// <summary>
/// create captcha text
/// </summary>
public interface ITextStrategy
{
    /// <summary>
    /// Retrieves a text with given options
    /// </summary>
    /// <param name="len">Len of desired text</param>
    /// <returns>text with given len</returns>
    TextProviderResultDto GetText(int len);
}

