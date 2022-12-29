using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock.Comparering
{
    public static class StockAttackComparerHelper
    {
        public static IStockComparisonAction? CompareAttack(IStockType leftType, IStockProperty? left, IStockType rightType, IStockProperty? right,IReadOnlyList<IStockAttack>? lefts, IReadOnlyList<IStockAttack>? rights)
        {
            if (lefts == null && rights == null)
            {
                return null;
            }
            if (lefts != null && rights != null)
            {
                var ups = rights.Except(lefts).ToList();
                var downs = lefts.Except(rights).ToList();
                if (ups.Count == 0 && downs.Count == 0)
                {
                    return null;
                }
                return new StockAttackChangedComparisonAction(lefts, rights, ups, downs,leftType,rightType,left,right);
            }
            return new StockAttackChangedComparisonAction(lefts, rights, rights, lefts, leftType, rightType, left, right);
        }
    }
}
