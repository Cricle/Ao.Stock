using System;
using System.Collections.Generic;
using System.Text;

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

        public string DebuggerView => ToString();

        public override int GetHashCode()
        {
            return StockTypeEqualityComparer.Default.GetHashCode(this);
        }

        public bool Equals(StockType? other)
        {
            return StockTypeEqualityComparer.Default.Equals(this, other);
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as StockType);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"({GetType().Name})Name: {Name ?? "(null)"}, Type:{(Type?.Name??"(null)")}");
            if (Attacks != null && Attacks.Count != 0)
            {
                sb.AppendLine($"\t[Attacks({Attacks.Count})]");
                for (int i = 0; i < Attacks.Count; i++)
                {
                    sb.AppendLine($"\t\t{Attacks[i]}");
                }
            }
            if (Properties != null && Properties.Count != 0)
            {
                sb.AppendLine($"\t[Properties({Properties.Count})]");
                for (int i = 0; i < Properties.Count; i++)
                {
                    sb.AppendLine($"\t\t{Properties[i].ToString()?.Replace("\n","\t")}");
                }
            }

            return sb.ToString();
        }
    }
}
