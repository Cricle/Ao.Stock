using System.Collections.Generic;

namespace Ao.Stock.Mirror
{
    public interface IQueryTranslateResult
    {
        string QueryString { get; }

        IReadOnlyDictionary<string, object> Args { get; }
    }
}
