using Ao.Stock.Comparering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Stock
{
    public class StockProperty : Named, IStockProperty, IEquatable<StockProperty>
    {
        public StockProperty()
        {
        }

        public StockProperty(Type? type, IReadOnlyList<IStockAttack>? attacks = null)
        {
            Type = type;
            Attacks = attacks;
        }

        public Type? Type { get; set; }

        public IReadOnlyList<IStockAttack>? Attacks { get; set; }

        public string DebuggerView => ToString();

        public override bool Equals(object? obj)
        {
            return Equals(obj as StockProperty);
        }
        public override int GetHashCode()
        {
            return StockPropertyEqualityComparer.Default.GetHashCode(this);
        }
        public bool Equals(StockProperty? other)
        {
            if (other == null)
            {
                return false;
            }
            return StockPropertyEqualityComparer.Default.Equals(this, other);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"({GetType().Name})Name: {Name ?? "(null)"}, Type: {(Type?.Name ?? "(null)")}");
            if (Attacks != null && Attacks.Count != 0)
            {
                sb.AppendLine($"\t[Attacks({Attacks.Count})]");
                for (int i = 0; i < Attacks.Count; i++)
                {
                    sb.AppendLine($"\t\t{Attacks[i]}");
                }
            }
            return sb.ToString();
        }
    }
}
