using Ao.Stock.Mirror;
using SqlKata;
using SqlKata.Compilers;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Kata
{
    public static class SqlKataQueryExtensions
    {
        public static SqlKataScope CreateScope(this Compiler compiler, DbConnection connection, bool toRawSql = true,int? commandTimeout=null)
        {
            return new SqlKataScope(compiler, connection, toRawSql,commandTimeout);
        }

        public static Task<int> ExecuteReadCountAsync(this DbConnection connection,
             Query query,
             Compiler compiler,
             int? commandTimeout = null,
             CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteReadCountAsync(result.Sql, result.NamedBindings,commandTimeout, token);
        }
        public static Task<List<IDictionary<string, object>>> ExecuteReaderAsync(this DbConnection connection,
             Query query,
             Compiler compiler,
             int? commandTimeout = null,
             CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteReaderAsync(result.Sql, result.NamedBindings, commandTimeout, token);
        }
        public static Task<TOutput> ExecuteReaderAsync<TOutput>(this DbConnection connection,
             Query query,
             Compiler compiler,
             IAsyncConverter<IDataReader, TOutput> converter,
             int? commandTimeout = null,
             CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteReaderAsync(result.Sql, converter, result.NamedBindings, commandTimeout, token);
        }
        public static Task<int> ExecuteNoQueryAsync(this DbConnection connection,
            IEnumerable<Query> queries, 
            Compiler compiler,
             int? commandTimeout = null,
            CancellationToken token = default)
        {
            var result = compiler.Compile(queries);
            return connection.ExecuteNonQueryAsync(result.Sql, result.NamedBindings, commandTimeout, token);
        }
        public static Task<int> ExecuteNoQueryAsync(this DbConnection connection,
            Query query,
            Compiler compiler,
            int? commandTimeout = null,
            CancellationToken token = default)
        {
            var result = compiler.Compile(query);
            return connection.ExecuteNonQueryAsync(result.Sql, result.NamedBindings, commandTimeout, token);
        }
    }
}
