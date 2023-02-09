using System.Collections.Generic;

namespace Ao.Stock.Explains
{
    public interface IExplainResult : IReadOnlyDictionary<string, object?>
    {
        void SetValue(string key, object? value);
    }
}
