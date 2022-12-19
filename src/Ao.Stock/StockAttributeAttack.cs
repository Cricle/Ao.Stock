using System;

namespace Ao.Stock
{
    public class StockAttributeAttack : IStockAttack, IEquatable<StockAttributeAttack>
    {
        public StockAttributeAttack()
        {
        }

        public StockAttributeAttack(Attribute? attribute)
        {
            Name = attribute?.GetType().Name;
            Attribute = attribute;
        }

        public StockAttributeAttack(string? name, Attribute? attribute)
        {
            Name = name;
            Attribute = attribute;
        }

        public string? Name { get; set; }

        public Attribute? Attribute { get; set; }

        public string DebuggerView => ToString();

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Attribute);
        }

        public override bool Equals(object? obj)
        {
            if (obj?.GetType() == typeof(StockAttributeAttack))
            {
                return Equals((StockAttributeAttack)obj);
            }
            return false;
        }

        public bool Equals(StockAttributeAttack? other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Name == Name && AttributeHelper.Equals(other.Attribute, Attribute);
        }
        public override string ToString()
        {
            return $"({GetType().Name})Name: {Name ?? "(null)"}, Attribute: {Attribute?.ToString() ?? "(null)"}";
        }
    }
}
