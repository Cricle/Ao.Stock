using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ao.Stock
{
    public class StockMapAttack : Dictionary<string, object>, IStockAttack
    {
        public StockMapAttack()
        {
        }

        public StockMapAttack(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        public StockMapAttack(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public StockMapAttack(int capacity) : base(capacity)
        {
        }

        public StockMapAttack(IDictionary<string, object> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        public StockMapAttack(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        protected StockMapAttack(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string? Name { get; set; }
    }
}
