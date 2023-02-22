using Ao.Stock.Mirror;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Kata
{
    public readonly struct SqlKataScope : ISqlKataScope, IDisposable
    {
        public SqlKataScope(Compiler compiler, DbConnection connection, bool toRawSql,int? timeout)
        {
            Compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            ToRawSql = toRawSql;
            CommandTimeout = timeout;
        }

        public Compiler Compiler { get; }

        public DbConnection Connection { get; }

        public bool ToRawSql { get; }

        public int? CommandTimeout { get; }

        public string CompileToSql(Query query)
        {
            return Compile(query).ToString();
        }
        public string CompileToSql(IEnumerable<Query> queries)
        {
            return Compile(queries).ToString();
        }
        public SqlResult Compile(Query query)
        {
            return Compiler.Compile(query);
        }

        public SqlResult Compile(IEnumerable<Query> queries)
        {
            return Compiler.Compile(queries);
        }

        public Task<int> ExecuteReadCountAsync(Query query,
            CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteReadCountAsync(result.ToString(), null,CommandTimeout, token);
            }
            return Connection.ExecuteReadCountAsync(result.Sql, result.NamedBindings, CommandTimeout, token);
        }
        public Task<List<IDictionary<string, object>>> ExecuteReaderAsync(Query query,
             CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteReaderAsync(result.ToString(), null, CommandTimeout, token);
            }
            return Connection.ExecuteReaderAsync(result.Sql, result.NamedBindings, CommandTimeout, token);
        }
        public Task<TOutput> ExecuteReaderAsync<TOutput>(Query query,
            IAsyncConverter<IDataReader, TOutput> converter,
            CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteReaderAsync(result.ToString(), converter, null, CommandTimeout, token);
            }
            return Connection.ExecuteReaderAsync(result.Sql, converter, result.NamedBindings, CommandTimeout, token);
        }
        public Task<List<TOutput>> ExecuteReaderAsync<TOutput>(Query query,
            CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteReaderAsync<TOutput>(result.ToString(), null, CommandTimeout, token);
            }
            return Connection.ExecuteReaderAsync<TOutput>(result.Sql, result.NamedBindings, CommandTimeout, token);
        }
        public Task<int> ExecuteNoQueryAsync(IEnumerable<Query> queries, CancellationToken token = default)
        {
            var result = Compile(queries);
            if (ToRawSql)
            {
                return Connection.ExecuteNonQueryAsync(result.ToString(), null, token:token);
            }
            return Connection.ExecuteNonQueryAsync(result.Sql, result.NamedBindings, token: token);
        }
        public Task<int> ExecuteNoQueryAsync(Query query, CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteNonQueryAsync(result.ToString(), null, token: token);
            }
            return Connection.ExecuteNonQueryAsync(result.Sql, result.NamedBindings, token: token);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
