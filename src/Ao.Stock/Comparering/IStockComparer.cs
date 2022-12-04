using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public interface IStockComparer<T>
    {
        IReadOnlyList<IStockComparisonAction> Compare(T left, T right);
    }
}
