using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public class StockProperty : Named, IStockProperty
    {
        public StockProperty()
        {
        }

        public StockProperty(Type? type, IReadOnlyList<IStockAttack>? attacks=null)
        {
            Type = type;
            Attacks = attacks;
        }

        public Type? Type { get; set; }

        public IReadOnlyList<IStockAttack>? Attacks { get; set; }
    }
}
