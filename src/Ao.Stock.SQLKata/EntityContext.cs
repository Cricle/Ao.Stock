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

        public Task<List<IDictionary<string, object>>> GetAsync(Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsDelete();
            query = func?.Invoke(query) ?? query;
            return Scope.ExecuteReaderAsync(query, token);
        }
        public Task<T> GetAsync<T>(IAsyncConverter<IDataReader,T> converter,Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsDelete();
            query = func?.Invoke(query) ?? query;
            return Scope.ExecuteReaderAsync(query, converter, token);
        }
        public Task<int> DeleteAsync(Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsDelete();
            query = func?.Invoke(query) ?? query;
            return Scope.ExecuteNoQueryAsync(query, token);
        }
        public Task<int> ExecuteAsync(string sql,IEnumerable<KeyValuePair<string,object>> args=null, CancellationToken token = default)
        {
            return Scope.Connection.ExecuteNoQueryAsync(sql, args, token);
        }
        public Task<List<IDictionary<string,object>>> QueryAsync(string sql, IEnumerable<KeyValuePair<string, object>> args = null, CancellationToken token = default)
        {
            return QueryAsync(sql, DictionaryReaderAsyncConverter.Instance, args, token);
        }
        public Task<List<T>> QueryAsync<T>(string sql, IEnumerable<KeyValuePair<string, object>> args = null, CancellationToken token = default)
        {
            return QueryAsync(sql, ORMAsyncConverter<T>.Default, args, token);
        }
        public Task<T> QueryAsync<T>(string sql,IAsyncConverter<IDataReader, T> converter, IEnumerable<KeyValuePair<string, object>> args = null, CancellationToken token = default)
        {
            return Scope.Connection.ExecuteReaderAsync(sql, converter, args, token);
        }

        public Task<int> InsertAsync(IEnumerable<string> keys, IEnumerable<IEnumerable<object>> values, Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsInsert(keys, values);
            query = func?.Invoke(query) ?? query;
            return Scope.ExecuteNoQueryAsync(query, token);
        }
        public Task<int> UpdateAsync(IEnumerable<string> keys, IEnumerable<object> values, Func<Query, Query> func = null, CancellationToken token = default)
        {
            var query = new Query(TableName).AsUpdate(keys, values);
            query = func?.Invoke(query) ?? query;
            return Scope.ExecuteNoQueryAsync(query, token);
        }
        public void Dispose()
        {
            Runtime.DbConnectionPool.Return(Scope.Connection);
            GC.SuppressFinalize(this);
        }
    }
}
