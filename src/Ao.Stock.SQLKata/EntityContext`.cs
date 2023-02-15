using Ao.Stock.Kata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SqlKata;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using Ao.Stock.Mirror;
using System.Data;
using System.ComponentModel;

namespace Ao.Stock.SQLKata
{
    public class EntityContext<T> : EntityContext
    {
        class PropertyVisitor
        {
            public readonly string Name;

            public readonly Func<T, object> getter;

            public readonly Action<T, object> setter;

            public PropertyVisitor(PropertyInfo info)
            {
                Name = info.GetCustomAttribute<ColumnAttribute>()?.Name ?? info.Name;
                {
                    var par1 = Expression.Parameter(typeof(T));
                    getter = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Call(par1, info.GetMethod), typeof(object)), par1).Compile();
                }
                {
                    var par1 = Expression.Parameter(typeof(T));
                    var par2 = Expression.Parameter(typeof(object));
                    var val = Expression.Convert(par2, info.PropertyType);
                    setter = Expression.Lambda<Action<T, object>>(Expression.Call(par1, info.SetMethod, val), par1, par2).Compile();
                }
            }
        }

        private static readonly PropertyVisitor[] visitors;
        private static readonly string[] names;

        static EntityContext()
        {
            visitors = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x =>
                {
                    var browser = x.GetCustomAttribute<BrowsableAttribute>();
                    return x.CanWrite && (browser == null || browser.Browsable);
                })
                .Select(x => new PropertyVisitor(x))
                .ToArray();
            names = visitors.Select(x => x.Name).ToArray();
        }

        public EntityContext(StockRuntime runtime, SqlKataScope scope, string tableName)
            : base(runtime, scope, tableName)
        {
        }

        public new Task<List<T>> GetAsync(Func<Query, Query> func = null, CancellationToken token = default)
        {
            return GetAsync(ORMAsyncConverter<T>.Default, func, token);
        }
        public async Task<List<T>> GetAsync(IAsyncConverter<IDataReader, List<T>> reader, Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName);
            query = func?.Invoke(query) ?? query;
            return await Scope.ExecuteReaderAsync(query, reader, token);
        }
        public async Task<int> InsertAsync(IEnumerable<T> entities, Func<Query, Query> func = null, CancellationToken token = default)
        {
            var values = entities.Select(x => visitors.Select(y => y.getter(x)));
            var query = new Query(TableName).AsInsert(names, values);
            query = func?.Invoke(query) ?? query;
            return await Scope.ExecuteNoQueryAsync(query, token);
        }
    }
}
