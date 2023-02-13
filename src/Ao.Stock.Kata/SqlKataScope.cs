using Ao.Stock.Mirror;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;

namespace Ao.Stock.Kata
{
    public readonly struct SqlKataScope : IDisposable
    {
        public SqlKataScope(Compiler compiler, DbConnection connection, bool toRawSql)
        {
            Compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            ToRawSql = toRawSql;
        }

        public Compiler Compiler { get; }

        public DbConnection Connection { get; }

        public bool ToRawSql { get; }

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
                return Connection.ExecuteReadCountAsync(result.ToString(), null, token);
            }
            return Connection.ExecuteReadCountAsync(result.Sql, result.NamedBindings, token);
        }
        public Task<List<IDictionary<string, object>>> ExecuteReaderAsync(Query query,
             CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteReaderAsync(result.ToString(), null, token);
            }
            return Connection.ExecuteReaderAsync(result.Sql, result.NamedBindings, token);
        }
        public Task<TOutput> ExecuteReaderAsync<TOutput>(Query query,
            IAsyncConverter<DbDataReader, TOutput> converter,
            CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteReaderAsync(result.ToString(), converter, null, token);
            }
            return Connection.ExecuteReaderAsync(result.Sql, converter, result.NamedBindings, token);
        }
        public Task<int> ExecuteNoQueryAsync(IEnumerable<Query> queries, CancellationToken token = default)
        {
            var result = Compile(queries);
            if (ToRawSql)
            {
                return Connection.ExecuteNoQueryAsync(result.ToString(), null, token);
            }
            return Connection.ExecuteNoQueryAsync(result.Sql, result.NamedBindings, token);
        }
        public Task<int> ExecuteNoQueryAsync(Query query, CancellationToken token = default)
        {
            var result = Compile(query);
            if (ToRawSql)
            {
                return Connection.ExecuteNoQueryAsync(result.ToString(), null, token);
            }
            return Connection.ExecuteNoQueryAsync(result.Sql, result.NamedBindings, token);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
