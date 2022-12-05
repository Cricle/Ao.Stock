using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public interface IStockPropertyComparer
    {
        IReadOnlyList<IStockComparisonAction> Compare(IStockType leftType, IStockProperty left, IStockType rightType, IStockProperty right);

    }
}
