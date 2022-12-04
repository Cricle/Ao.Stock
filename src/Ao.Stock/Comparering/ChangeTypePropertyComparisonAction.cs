using System;

namespace Ao.Stock.Comparering
{
    public class StockPropertyTypeChangedComparisonAction : StockPropertyComparisonAction
    {
        public StockPropertyTypeChangedComparisonAction(IStockType leftStockType, 
            IStockType rightStockType, 
            IStockProperty left, 
            IStockProperty right,
            Type? leftType,
            Type? rightType)
            : base(leftStockType, rightStockType, left, right)
        {
            LeftPropertyType = leftType;
            RightPropertyType = rightType;
        }

        public Type? LeftPropertyType { get; }

        public Type? RightPropertyType { get; }
    }
}
