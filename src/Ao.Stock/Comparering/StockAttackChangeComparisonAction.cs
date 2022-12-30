using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public class StockAttackChangedComparisonAction : StockComparisonAction
    {
        public StockAttackChangedComparisonAction(IReadOnlyList<IStockAttack>? lefts,
            IReadOnlyList<IStockAttack>? rights,
            IReadOnlyList<IStockAttack>? ups,
            IReadOnlyList<IStockAttack>? downs,
            IStockType leftType,
            IStockType rightType,
            IStockProperty? leftProperty,
            IStockProperty? rightProperty)
        {
            Lefts = lefts;
            Rights = rights;
            Ups = ups;
            Downs = downs;
            LeftType = leftType;
            RightType = rightType;
            LeftProperty = leftProperty;
            RightProperty = rightProperty;
        }

        public IReadOnlyList<IStockAttack>? Lefts { get; }

        public IReadOnlyList<IStockAttack>? Rights { get; }

        public IReadOnlyList<IStockAttack>? Ups { get; }

        public IReadOnlyList<IStockAttack>? Downs { get; }

        public IStockType LeftType { get; }

        public IStockType RightType { get; }

        public IStockProperty? LeftProperty { get; }

        public IStockProperty? RightProperty { get; }

    }
}
