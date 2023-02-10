using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ao.Stock.Explains
{
    public abstract class ExplainResultBase<TResult> : IExplainResult
    {
        class PropertyMap
        {
            public readonly string Name;

            public readonly Type ActualType;

            public readonly Func<TResult, object>? Getter;

            public readonly Action<TResult, object>? Setter;

            public PropertyMap(string name, Type actualType, Func<TResult, object>? getter, Action<TResult, object>? setter)
            {
                Name = name;
                ActualType = actualType;
                Getter = getter;
                Setter = setter;
            }
        }
        private static readonly List<PropertyMap> valueMaps;
        private static readonly Type type = typeof(TResult);

        [Browsable(false)]
        public IEnumerable<string> Keys => valueMaps.Select(x => x.Name);

        [Browsable(false)]
        public IEnumerable<object> Values => valueMaps.Select(x => x.Getter((TResult)(object)this));

        [Browsable(false)]
        public int Count => valueMaps.Count;

        [Browsable(false)]
        protected string DebuggerView => ToString();

        [Browsable(false)]
        public object this[string key]
        {
            get
            {
                var val = valueMaps.First(x => x.Name == key);
                if (val.Getter == null)
                {
                    throw new InvalidOperationException($"Property {key} can't get");
                }
                return val.Getter((TResult)(object)this);
            }
        }

        static ExplainResultBase()
        {
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x =>
                {
                    var attr = x.GetCustomAttribute<BrowsableAttribute>();
                    if (attr == null)
                    {
                        return true;
                    }
                    return attr.Browsable;
                })
                .ToArray();
            valueMaps = new List<PropertyMap>();
            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var name = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
                Action<TResult, object>? setter = null;
                Func<TResult, object>? getter = null;
                if (prop.CanRead)
                {
                    getter = CompileGetter(type, prop);
                }
                if (prop.CanWrite)
                {
                    setter = CompileSetter(type, prop);
                }
                valueMaps.Add(new PropertyMap(name, prop.PropertyType, getter, setter));
            }
        }
        private static Action<TResult, object> CompileSetter(Type type, PropertyInfo property)
        {
            var par = Expression.Parameter(type);
            var par1 = Expression.Parameter(typeof(object));
            return Expression.Lambda<Action<TResult, object>>(
                Expression.Call(par, property.SetMethod!,
                    Expression.Convert(par1, property.PropertyType)), par, par1).Compile();
        }
        private static Func<TResult, object> CompileGetter(Type type, PropertyInfo property)
        {
            var par = Expression.Parameter(type);
            return Expression.Lambda<Func<TResult, object>>(
                Expression.Convert(Expression.Call(par, property.GetMethod!), typeof(object)), par).Compile();
        }

        public bool ContainsKey(string key)
        {
            return valueMaps.Any(x => x.Name == key);
        }

        public bool TryGetValue(string key, out object? value)
        {
            var propMap = valueMaps.FirstOrDefault(x => x.Name == key);
            if (propMap == null)
            {
                value = null;
                return false;
            }
            if (propMap.Getter == null)
            {
                throw new InvalidOperationException($"Property {key} can't get");
            }
            value = propMap.Getter((TResult)(object)this);
            return true;
        }

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            return new Enumerator(valueMaps, (TResult)(object)this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void SetValue(string key, object? value)
        {
            var propMap = valueMaps.FirstOrDefault(x => x.Name == key);
            if (propMap == null)
            {
                throw new KeyNotFoundException(key);
            }
            if (propMap.Setter == null)
            {
                throw new InvalidOperationException($"Property {key} can't set");
            }
            propMap.Setter((TResult)(object)this, Convert.ChangeType(value, propMap.ActualType));
        }

        public override string ToString()
        {
            var blocks = new string[Count];
            var idx = 0;
            foreach (var item in this)
            {
                blocks[idx++] = $"{item.Key}={item.Value}";
            }
            return string.Join(", ", blocks);
        }

        class Enumerator : IEnumerator<KeyValuePair<string, object?>>
        {
            private readonly List<PropertyMap> maps;
            private readonly TResult result;
            private PropertyMap? map;
            private int index;

            public Enumerator(List<ExplainResultBase<TResult>.PropertyMap> maps, TResult result)
            {
                this.maps = maps;
                this.result = result;
                Reset();
            }

            public KeyValuePair<string, object?> Current => new KeyValuePair<string, object?>(map!.Name, map!.Getter(result));

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (index < maps.Count)
                {
                    map = maps[index];
                    index++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                map = null;
                index = 0;
            }
        }
    }
}
