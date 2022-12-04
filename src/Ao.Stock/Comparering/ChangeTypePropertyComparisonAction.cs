using System;

namespace Ao.Stock.Comparering
{
    public class StockPropertyTypeChangedComparisonAction : StockPropertyComparisonAction
    {
        public StockPropertyTypeChangedComparisonAction(IStockProperty left, IStockProperty right, Type? leftType, Type? rightType)
            : base(left, right)
        {
            LeftType = leftType;
            RightType = rightType;
        }

        public Type? LeftType { get; }

        public Type? RightType { get; }
    }
}
