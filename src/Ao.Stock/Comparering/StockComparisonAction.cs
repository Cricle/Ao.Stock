using System;

namespace Ao.Stock.Comparering
{
    public class StockComparisonAction : IStockComparisonAction
    {
        public virtual long Id =>BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
    }
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
    public class StockPropertyComparisonAction : StockComparisonAction
    {
        public StockPropertyComparisonAction(IStockProperty left, IStockProperty right)
        {
            Left = left;
            Right = right;
        }

        public IStockProperty Left { get; }

        public IStockProperty Right { get; }
    }
}
