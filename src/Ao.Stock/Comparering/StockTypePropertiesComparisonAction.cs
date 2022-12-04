using System.Collections.Generic;

namespace Ao.Stock.Comparering
{
    public class StockTypePropertiesComparisonAction : StockTypeComparisonAction
    {
        public StockTypePropertiesComparisonAction(IStockType left, 
            IStockType right, 
            IReadOnlyList<IStockProperty>? leftProps,
            IReadOnlyList<IStockProperty>? rightProps)
            : base(left, right)
        {
            LeftProperies = leftProps;
            RightProperies = rightProps;
        }

        public IReadOnlyList<IStockProperty>? LeftProperies { get; }

        public IReadOnlyList<IStockProperty>? RightProperies { get; }

    }
}
