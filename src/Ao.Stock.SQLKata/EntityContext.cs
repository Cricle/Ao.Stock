using Ao.Stock.Kata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SqlKata;
using Ao.Stock.Mirror;
using System.Data;

namespace Ao.Stock.SQLKata
{
    public class EntityContext : IDisposable
    {
        public EntityContext(StockRuntime runtime, SqlKataScope scope, string tableName)
        {
            Runtime = runtime;
            Scope = scope;
            TableName = tableName;
        }

        public StockRuntime Runtime { get; }

        public SqlKataScope Scope { get; }

        public string TableName { get; }

        public async Task<List<IDictionary<string, object>>> GetAsync(Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsDelete();
            query = func?.Invoke(query) ?? query;
            return await Scope.ExecuteReaderAsync(query, token);
        }
        public async Task<T> GetAsync<T>(IAsyncConverter<IDataReader,T> converter,Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsDelete();
            query = func?.Invoke(query) ?? query;
            return await Scope.ExecuteReaderAsync(query, converter, token);
        }
        public async Task<int> DeleteAsync(Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsDelete();
            query = func?.Invoke(query) ?? query;
            return await Scope.ExecuteNoQueryAsync(query, token);
        }

        public async Task<int> InsertAsync(IEnumerable<string> keys, IEnumerable<IEnumerable<object>> values, Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsInsert(keys, values);
            query = func?.Invoke(query) ?? query;
            return await Scope.ExecuteNoQueryAsync(query, token);
        }
        public async Task<int> UpdateAsync(IEnumerable<string> keys, IEnumerable<object> values, Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsUpdate(keys, values);
            query = func?.Invoke(query) ?? query;
            return await Scope.ExecuteNoQueryAsync(query, token);
        }
        public void Dispose()
        {
            Runtime.DbConnectionPool.Return(Scope.Connection);
            GC.SuppressFinalize(this);
        }
    }
}
