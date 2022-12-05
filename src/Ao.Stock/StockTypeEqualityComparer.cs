using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public class StockTypeEqualityComparer : IEqualityComparer<IStockType>
    {
        public static readonly StockTypeEqualityComparer Default = new StockTypeEqualityComparer();

        public bool Equals(IStockType? x, IStockType? y)
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
            if (x.Properties == null && y.Properties == null &&
                x.Attacks == null && y.Attacks == null)
            {
                return true;
            }
            if (x.Properties != null || y.Properties != null)
            {
                return false;
            }
            if (x.Attacks != null || y.Attacks != null)
            {
                return false;
            }
            if (x.Properties != null)
            {
                if (x.Properties.Count != y.Properties!.Count)
                {
                    return false;
                }
                for (int i = 0; i < x.Properties.Count; i++)
                {
                    if (!x.Properties[i].Equals(y.Properties[i]))
                    {
                        return false;
                    }
                }
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

        public int GetHashCode(IStockType obj)
        {
            if (obj == null)
            {
                return 0;
            }
            var hc = new HashCode();
            hc.Add(obj.Type);
            if (obj.Properties != null)
            {
                for (int i = 0; i < obj.Properties.Count; i++)
                {
                    hc.Add(obj.Properties[i]);
                }
            }
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
