using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Ao.Stock.Mirror
{
    public abstract class SQLCopyingBase
    {
        protected SQLCopyingBase(ISQLDatabaseInfo source, ISQLDatabaseInfo destination)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public ISQLDatabaseInfo Source { get; }

        public ISQLDatabaseInfo Destination { get; }

        public ISQLTableSelector? TableSelector { get; set; }

        public bool CleanTable { get; set; }

        public int BatchSize { get; set; } = 100;

        public async Task RunAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            await OnRunningAsync(token);
            if (Source.DbConnection.State != ConnectionState.Open)
            {
                await Source.DbConnection.OpenAsync(token).ConfigureAwait(false);
            }
            if (Destination.DbConnection.State != ConnectionState.Open)
            {
                await Destination.DbConnection.OpenAsync(token).ConfigureAwait(false);
            }
            token.ThrowIfCancellationRequested();
            var tables = await GetTablesAsync(token);
            if (CleanTable)
            {
                await ClearTablesAsync(Destination, tables);
            }
            await CopyAsync(Source.DbConnection, Destination.DbConnection, tables, token);
        }

        protected virtual Task OnRunningAsync(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }

        protected abstract Task<IEnumerable<string>> GetTablesAsync(CancellationToken token = default);

        protected virtual string GenerateDeleteSql(ISQLDatabaseInfo info, string tableName)
        {
            return $"DELETE FROM {info.MethodWrapper.Quto(tableName)}";
        }
        protected virtual async Task ClearTablesAsync(ISQLDatabaseInfo info, IEnumerable<string> tables)
        {
            foreach (var item in tables)
            {
                var sql = GenerateDeleteSql(info, item);
                await info.DbConnection.ExecuteNonQueryAsync(sql);
            }
        }

        protected virtual async Task CopyAsync(DbConnection sourceConn, DbConnection destConn, IEnumerable<string> tables, CancellationToken token)
        {
            foreach (var item in tables)
            {
                token.ThrowIfCancellationRequested();
                using (var sourceCommand = sourceConn.CreateCommand())
                {
                    sourceCommand.CommandText = Source.CreateQuerySql(item);
                    using (var reader = sourceCommand.ExecuteReader())
                    {
                        var named = Destination.MethodWrapper.Quto(item);
                        var cp = new SQLMirrorCopy(reader,
                            new QueryTranslateResult(sourceCommand.CommandText),
                            new SQLMirrorTarget(destConn, named),
                            Destination.MethodWrapper,
                            BatchSize);
                        await cp.CopyAsync();
                    }
                }
            }

        }
    }
}
