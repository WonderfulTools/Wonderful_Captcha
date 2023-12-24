using Microsoft.Extensions.DependencyInjection;

namespace Core.Utils;

public class Lazier<T> : Lazy<T> where T : class 
{
    public Lazier(IServiceProvider provider) : base(provider.GetRequiredService<T>) 
    { }
}