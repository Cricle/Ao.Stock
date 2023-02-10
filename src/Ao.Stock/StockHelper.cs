using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ao.Stock
{
    public static class StockHelper
    {
        public static IStockType FromType(Type type)
        {
            return FromType(type, (ReflectionOptions?)null);
        }
        public static IStockType FromType<T>(string typeName)
        {
            return FromType(typeof(T), typeName);
        }
        public static IStockType FromType(Type type, string typeName)
        {
            return FromType(type, new ReflectionOptions { TypeNameGetter = _ => typeName });
        }
        public static IStockType FromType<T>(ReflectionOptions? options)
        {
            return FromType(typeof(T), options);
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
                    if (item.GetType().GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                    {
                        attacks.Add(FromAttribute(item));
                    }
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
