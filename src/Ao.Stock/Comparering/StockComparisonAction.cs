using System;

namespace Ao.Stock.Comparering
{
    public class StockComparisonAction : IStockComparisonAction
    {
        public virtual long Id => BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
    }
}
