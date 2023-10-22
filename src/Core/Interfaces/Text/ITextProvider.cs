namespace WonderfulCaptcha.Text;

/// <summary>
///  This is responsible for giving write ITextStrategy
/// </summary>
public interface ITextProvider
{
    /// <summary>Retrieves a specific ITextStrategy Instance by its type</summary>
    /// <param name="type">The type of the strategy to retrieve</param>
    /// <returns>An ITextStrategy definition</returns>
    ITextStrategy GetInstance(StrategyEnum type);
}

