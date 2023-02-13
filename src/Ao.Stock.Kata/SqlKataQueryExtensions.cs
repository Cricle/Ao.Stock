using Ao.Stock.Mirror;
using SqlKata;
using SqlKata.Compilers;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;

namespace Ao.Stock.Kata
{
    public static class SqlKataQueryExtensions
    {
        public static SqlKataScope CreateScope(this Compiler compiler,DbConnection connection,bool toRawSql=true)
        {
            return new SqlKataScope(compiler, connection, toRawSql);
        }

        public static Task<int> ExecuteReadCountAsync(this DbConnection connection,
             Query query,
             Compiler compiler,
             CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteReadCountAsync(result.Sql, result.NamedBindings, token);
        }
        public static Task<List<IDictionary<string, object>>> ExecuteReaderAsync(this DbConnection connection,
             Query query,
             Compiler compiler,
             CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteReaderAsync(result.Sql, result.NamedBindings, token);
        }
        public static Task<TOutput> ExecuteReaderAsync<TOutput>(this DbConnection connection,
             Query query,
             Compiler compiler,
             IAsyncConverter<DbDataReader, TOutput> converter,
             CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteReaderAsync(result.Sql, converter, result.NamedBindings, token);
        }
        public static Task<int> ExecuteNoQueryAsync(this DbConnection connection, IEnumerable<Query> queries, Compiler compiler, CancellationToken token = default)
        {
            var result = compiler.Compile(queries);
            return connection.ExecuteNoQueryAsync(result.Sql, result.NamedBindings, token);
        }
        public static Task<int> ExecuteNoQueryAsync(this DbConnection connection, Query query, Compiler compiler, CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteNoQueryAsync(result.Sql, result.NamedBindings, token);
        }
    }
}
