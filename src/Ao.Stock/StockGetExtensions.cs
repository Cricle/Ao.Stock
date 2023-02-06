using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ao.Stock
{
    public static class StockGetExtensions
    {
        public static IEnumerable<object?> Convert(this IStockType type,
            IEnumerable<object?> inputs)
        {
            return Convert(type, type.Properties.Select(x=>x.Name), inputs, DefaultStockConvertOptions.Default, DefaultCastHelper.Default);
        }
        public static IEnumerable<object?> Convert(this IStockType type,
            IEnumerable<string> propertyNames,
            IEnumerable<object?> inputs)
        {
            return Convert(type, propertyNames, inputs, DefaultStockConvertOptions.Default, DefaultCastHelper.Default);
        }
        public static IEnumerable<object?> Convert(this IStockType type,
            IEnumerable<string> propertyNames,
            IEnumerable<object?> inputs,
            IStockConvertOptions options)
        {
            return Convert(type,propertyNames,inputs,options,DefaultCastHelper.Default);
        }
        public static IEnumerable<object?> Convert(this IStockType type,
            IEnumerable<string> propertyNames,
            IEnumerable<object?> inputs,
            IStockConvertOptions options,
            ICastHelper castHelper)
        {
            if (type.Properties == null || type.Properties.Count == 0)
            {
                yield break;
            }
            using (var enuName = propertyNames.GetEnumerator())
            using (var enuInput = inputs.GetEnumerator())
            {
                while (enuName.MoveNext()&& enuInput.MoveNext())
                {
                    var prop = options.FindProperty(type, enuName.Current);
                    if (prop == null)
                    {
                        throw new ArgumentException($"Property {enuName.Current} not found in type {type.Name}");
                    }
                    if (TryConvert(prop, enuInput.Current, castHelper, out var output, out var ex))
                    {
                        yield return output;
                    }
                    else
                    {
                        yield return options.FailCast(enuInput.Current, ex);
                    }
                }
            }
        }
        public static object? Convert(this IStockProperty property, object input, bool throwException = true)
        {
            if (TryConvert(property,input,out var output,out var ex))
            {
                return output;
            }
            if (throwException)
            {
                throw ex;
            }
            return null;
        }
        public static bool TryConvert(this IStockProperty property, object? input, out object? output)
        {
            return TryConvert(property, input, DefaultCastHelper.Default, out output, out _);
        }
        public static bool TryConvert(this IStockProperty property, object? input, out object? output, out Exception? exception)
        {
            return TryConvert(property, input,DefaultCastHelper.Default, out output, out exception);
        }
        public static bool TryConvert(this IStockProperty property,object? input,ICastHelper castHelper,out object? output,out Exception? exception)
        {
            ThrowIfNoType(property);

            return castHelper.TryConvert(input,property.Type!, out output, out exception);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowIfNoType(IStockProperty property) 
        {
            if (property.Type==null)
            {
                throw new ArgumentException("property.Type is null");
            }
        }
        public static bool HasAttributeAttack<T>(this IStockAttachable attachable)
        {
            return HasAttributeAttack(attachable, typeof(T));
        }
        public static bool HasAttributeAttack(this IStockAttachable attachable, Type attributeType)
        {
            return GetAttributeAttacks(attachable).Any(x => attributeType == x.Attribute?.GetType());
        }
        public static StockAttributeAttack? GetAttributeAttack(this IStockAttachable attachable)
        {
            return GetAttributeAttacks(attachable).FirstOrDefault();
        }
        public static IEnumerable<StockAttributeAttack> GetAttributeAttacks(this IStockAttachable attachable)
        {
            return GetAttacks(attachable, x => x is StockAttributeAttack).OfType<StockAttributeAttack>();
        }

        public static bool HasAttack(this IStockAttachable attachable, Func<IStockAttack, bool> attackCondition)
        {
            return GetAttacks(attachable, attackCondition).Any();
        }
        public static IStockAttack? GetAttack(this IStockAttachable attachable, Func<IStockAttack, bool> attackCondition)
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
        public static IStockProperty? GetProperty(this IStockType type, string propertyType, StringComparison comparison = StringComparison.Ordinal)
        {
            return GetProperty(type, x => string.Equals(x.Name, propertyType, comparison));
        }
        public static IStockProperty? GetProperty(this IStockType type, Func<IStockProperty, bool> propertyCondition)
        {
            return GetProperties(type, propertyCondition).FirstOrDefault();
        }
        public static IEnumerable<IStockProperty> GetProperties(this IStockType type, Func<IStockProperty, bool> propertyCondition)
        {
            if (type.Properties == null)
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
            if (type.Properties == null)
            {
                return Array.Empty<string?>();
            }
            var names = new string?[type.Properties.Count];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = type.Properties[i].Name;
            }
            return names;
        }
    }
}
