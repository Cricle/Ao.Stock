using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public class StockPropertyEqualityComparer : IEqualityComparer<IStockProperty>
    {
        public static readonly StockPropertyEqualityComparer Default = new StockPropertyEqualityComparer();

        public bool Equals(IStockProperty x, IStockProperty y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            if (x.Type != y.Type || x.Name != y.Name)
            {
                return false;
            }
            if (x.Attacks == null && y.Attacks == null)
            {
                return true;
            }
            if (x.Attacks != null || y.Attacks != null)
            {
                return false;
            }
            if (x.Attacks != null)
            {
                if (x.Attacks.Count != y.Attacks!.Count)
                {
                    return false;
                }
                for (int i = 0; i < x.Attacks.Count; i++)
                {
                    if (!x.Attacks[i].Equals(y.Attacks[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int GetHashCode(IStockProperty obj)
        {
            if (obj == null)
            {
                return 0;
            }
            var hc = new HashCode();
            hc.Add(obj.Type);
            if (obj.Attacks != null)
            {
                for (int i = 0; i < obj.Attacks.Count; i++)
                {
                    hc.Add(obj.Attacks[i]);
                }
            }
            return hc.ToHashCode();
        }
    }
}
