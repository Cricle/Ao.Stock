using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public class StockAttackChangeComparisonAction : StockComparisonAction
    {
        public StockAttackChangeComparisonAction(IReadOnlyList<IStockAttack>? lefts,
            IReadOnlyList<IStockAttack>? rights,
            IReadOnlyList<IStockAttack>? ups,
            IReadOnlyList<IStockAttack>? downs)
        {
            Lefts = lefts;
            Rights = rights;
            Ups = ups;
            Downs = downs;
        }

        public IReadOnlyList<IStockAttack>? Lefts { get; }

        public IReadOnlyList<IStockAttack>? Rights { get; }

        public IReadOnlyList<IStockAttack>? Ups { get; }

        public IReadOnlyList<IStockAttack>? Downs { get; }
    }
}
