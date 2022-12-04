using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public interface IStockTypeComparer
    {
        IReadOnlyList<IStockComparisonAction> Compare(IStockType left, IStockType right);
    }
}
