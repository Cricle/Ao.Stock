using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock.Comparering
{
    public static class StockAttackComparerHelper
    {
        public static IStockComparisonAction? CompareAttack(IReadOnlyList<IStockAttack>? lefts, IReadOnlyList<IStockAttack>? rights)
        {
            if (lefts == null && rights == null)
            {
                return null;
            }
            if (lefts != null && rights != null)
            {
                var ups = rights.Except(lefts).ToList();
                var downs = lefts.Except(rights).ToList();
                return new StockAttackChangeComparisonAction(lefts, rights, ups, downs);
            }
            return new StockAttackChangeComparisonAction(lefts, rights, rights, lefts);
        }
    }
}
