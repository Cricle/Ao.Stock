namespace Ao.Stock.Comparering
{
    public class StockTypeComparisonAction : StockComparisonAction
    {
        public StockTypeComparisonAction(IStockType left, IStockType right)
        {
            Left = left;
            Right = right;
        }

        public IStockType Left { get; }

        public IStockType Right { get; }
    }

    public class StockCreateTypeComparisonAction : StockTypeComparisonAction
    {
        public StockCreateTypeComparisonAction(IStockType? left, IStockType right) : base(left, right)
        {
        }
    }
    public class StockDropTypeComparisonAction : StockTypeComparisonAction
    {
        public StockDropTypeComparisonAction(IStockType left, IStockType? right) : base(left, right)
        {
        }
    }
}
