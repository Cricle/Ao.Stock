using System.Collections.Generic;

namespace Ao.Stock.Querying
{
    public interface IMethodMetadata : IQueryMetadata, IExpressionTypeProvider
    {
        string Method { get; }

        IList<IQueryMetadata>? Args { get; }
    }
}
