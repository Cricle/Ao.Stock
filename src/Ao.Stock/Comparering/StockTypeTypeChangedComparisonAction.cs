using System;

namespace Ao.Stock.Comparering
{
    public class StockTypeTypeChangedComparisonAction : StockTypeComparisonAction
    {
        public StockTypeTypeChangedComparisonAction(IStockType left, IStockType right, Type? leftType, Type? rightType)
            : base(left, right)
        {
            LeftType = leftType;
            RightType = rightType;
        }

        public Type? LeftType { get; }

        public Type? RightType { get; }

    }
}
