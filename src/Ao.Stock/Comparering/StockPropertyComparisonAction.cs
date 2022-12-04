namespace Ao.Stock.Comparering
{
    public class StockPropertyComparisonAction : StockComparisonAction
    {
        public StockPropertyComparisonAction(IStockType leftType, IStockType rightType, IStockProperty left, IStockProperty right)
        {
            LeftType = leftType;
            RightType = rightType;
            Left = left;
            Right = right;
        }

        public IStockType LeftType { get; }

        public IStockType RightType { get; }

        public IStockProperty Left { get; }

        public IStockProperty Right { get; }
    }
}
