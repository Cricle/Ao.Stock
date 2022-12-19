using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock
{
    public static class StockGetExtensions
    {
        public static bool HasAttributeAttack<T>(this IStockAttachable attachable)
        {
            return HasAttributeAttack(attachable,typeof(T));
        }
        public static bool HasAttributeAttack(this IStockAttachable attachable,Type attributeType)
        {
            return GetAttributeAttacks(attachable).Any(x => attributeType == x.GetType());
        }
        public static StockAttributeAttack GetAttributeAttack(this IStockAttachable attachable)
        {
            return GetAttributeAttacks(attachable).FirstOrDefault();
        }
        public static IEnumerable<StockAttributeAttack> GetAttributeAttacks(this IStockAttachable attachable)
        {
            return GetAttacks(attachable,x=>x is StockAttributeAttack).OfType<StockAttributeAttack>();
        }

        public static bool HasAttack(this IStockAttachable attachable, Func<IStockAttack, bool> attackCondition)
        {
            return GetAttacks(attachable,attackCondition).Any();
        }
        public static IStockAttack GetAttack(this IStockAttachable attachable, Func<IStockAttack, bool> attackCondition)
        {
            return GetAttacks(attachable, attackCondition).FirstOrDefault();
        }
        public static IEnumerable<IStockAttack> GetAttacks(this IStockAttachable attachable, Func<IStockAttack, bool> attackCondition)
        {
            if (attachable.Attacks == null)
            {
                yield break;
            }
            for (int i = 0; i < attachable.Attacks.Count; i++)
            {
                var prop = attachable.Attacks[i];
                if (attackCondition(prop))
                {
                    yield return prop;
                }
            }
        }
        public static bool HasProperty(this IStockType type, Func<IStockProperty, bool> propertyCondition)
        {
            return GetProperties(type, propertyCondition).Any();
        }
        public static IStockProperty? GetProperty(this IStockType type, string propertyType, StringComparison comparison= StringComparison.Ordinal)
        {
            return GetProperty(type, x => string.Equals(x.Name, propertyType, comparison));
        }
        public static IStockProperty? GetProperty(this IStockType type, Func<IStockProperty, bool> propertyCondition)
        {
            return GetProperties(type, propertyCondition).FirstOrDefault();
        }
        public static IEnumerable<IStockProperty> GetProperties(this IStockType type,Func<IStockProperty,bool> propertyCondition)
        {
            if (type.Properties==null)
            {
                yield break;
            }
            for (int i = 0; i < type.Properties.Count; i++)
            {
                var prop = type.Properties[i];
                if (propertyCondition(prop))
                {
                    yield return prop;
                }
            }
        }
        public static IReadOnlyList<string?> GetPropertyNames(this IStockType type)
        {
            if (type.Properties==null)
            {
                return Array.Empty<string?>();
            }
            var names=new string?[type.Properties.Count];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = type.Properties[i].Name;
            }
            return names;
        }
    }
}
