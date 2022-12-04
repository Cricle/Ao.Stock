using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public class StockType : Named, IStockType, IEquatable<StockType>
    {
        public StockType()
        {
        }

        public StockType(Type? type, IReadOnlyList<IStockAttack>? attacks = null, IReadOnlyList<IStockProperty>? properties = null)
        {
            Type = type;
            Attacks = attacks;
            Properties = properties;
        }

        public Type? Type { get; set; }

        public IReadOnlyList<IStockAttack>? Attacks { get; set; }

        public IReadOnlyList<IStockProperty>? Properties { get; set; }

        public override int GetHashCode()
        {
            return StockTypeEqualityComparer.Default.GetHashCode(this);
        }

        public bool Equals(StockType? other)
        {
            return StockTypeEqualityComparer.Default.Equals(this, other);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as StockType);
        }
    }
}
