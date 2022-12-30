using System.Collections.Generic;

namespace Ao.Stock
{
    public class StockIntangibleGroup : List<IStockIntangible>, IStockIntangible
    {
        public StockIntangibleGroup()
        {
        }

        public StockIntangibleGroup(IEnumerable<IStockIntangible> collection) : base(collection)
        {
        }

        public StockIntangibleGroup(int capacity) : base(capacity)
        {
        }

        public void Config<T>(ref T input, IIntangibleContext? context)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Config(ref input, context);
            }
        }

        public T Get<T>(IIntangibleContext? context)
        {
            for (int i = 0; i < Count; i++)
            {
                var val = this[i].Get<T>(context);
                if (!IsEmpty(val))
                {
                    return val;
                }
            }
            return default;
        }

        protected virtual bool IsEmpty<T>(T val)
        {
            return val == null;
        }
    }
}
