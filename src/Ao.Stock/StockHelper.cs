using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ao.Stock
{
    public static class StockHelper
    {
        public static IStockType FromType(Type type)
        {
            return FromType(type, null);
        }
        public static IStockType FromType(Type type, ReflectionOptions? options)
        {
            var t = new StockType
            {
                Name = options?.TypeNameGetter?.Invoke(type) ?? type?.Name,
                Type = type,
            };
            if (type != null)
            {
                var attacks = new List<IStockAttack>(0);
                var props = new List<IStockProperty>(0);
                t.Attacks = attacks;
                t.Properties = props;
                foreach (Attribute item in type.GetCustomAttributes(options?.AttributeInherit ?? false))
                {
                    attacks.Add(FromAttribute(item));
                }
                foreach (var item in type.GetProperties())
                {
                    props.Add(FromProperty(item, options));
                }
            }

            return t;
        }
        public static IStockProperty FromProperty(PropertyInfo? info, ReflectionOptions? options)
        {
            var prop = new StockProperty
            {
                Name = options?.PropertyNameGetter?.Invoke(info) ?? info?.Name,
                Type = info?.PropertyType,
            };
            if (info != null)
            {
                var attacks = new List<IStockAttack>(0);
                prop.Attacks = attacks;
                foreach (Attribute item in info.GetCustomAttributes(options?.AttributeInherit ?? false))
                {
                    attacks.Add(FromAttribute(item));
                }
            }
            return prop;
        }
        public static IStockAttack FromAttribute(Attribute? attribute)
        {
            return new StockAttributeAttack(attribute);
        }
    }
}
