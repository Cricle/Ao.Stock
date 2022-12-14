using Ao.Stock.Kata.Mirror;
using Ao.Stock.Mirror;
using Ao.Stock.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SqlKata;
using System.Data;
using System.Data.Common;

namespace Ao.Stock.Kata.Copying
{
    public class SQLCopying
    {
        public SQLCopying(ISQLDatabaseInfo source, ISQLDatabaseInfo destination)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public ISQLDatabaseInfo Source { get; }

        public ISQLDatabaseInfo Destination { get; }

        public ISQLTableSelector? TableSelector { get; set; }

        public bool SynchronousStructure { get; set; } = true;

        public bool CleanTable { get; set; }

        public int BatchSize { get; set; } = 100;

        protected virtual async Task SynchronousStructureAsync()
        {
            using (var sourceMig = Source.StockIntangible.Get<AutoMigrationHelper>(Source.Context))
            {
                var sourceModel = sourceMig.ScaffoldHelper.Scaffold();
                var destDbOptionsDest = new DbContextOptionsBuilder();
                Destination.StockIntangible.Config(ref destDbOptionsDest, Destination.Context);
                destDbOptionsDest.UseModel(sourceModel);
                var options = destDbOptionsDest.Options;
                options.GetExtension<CoreOptionsExtension>().WithServiceProviderCachingEnabled(false);
                var dbDesk = new DbContext(options);
                await dbDesk.Database.EnsureDeletedAsync();
                await dbDesk.Database.EnsureCreatedAsync();
            }
        }

        public async Task RunAsync(CancellationToken token = default)
        {
            using (var connSource = Source.StockIntangible.Get<DbConnection>(Source.Context))
            using (var connDest = Destination.StockIntangible.Get<DbConnection>(Destination.Context))
            {
                if (SynchronousStructure)
                {
                    await SynchronousStructureAsync();
                }
                token.ThrowIfCancellationRequested();
                if (connSource.State != ConnectionState.Open)
                {
                    await connSource.OpenAsync().ConfigureAwait(false);
                }
                if (connDest.State != ConnectionState.Open)
                {
                    await connDest.OpenAsync().ConfigureAwait(false);
                }
                token.ThrowIfCancellationRequested();
                var tables = await GetTablesAsync();
                if (CleanTable&&!SynchronousStructure)
                {
                    await ClearTablesAsync(connDest,Destination,tables);
                }
                await CopyAsync(connSource, connDest, tables, token);
            }
        }
        protected virtual async Task<IEnumerable<string>> GetTablesAsync()
        {
            using (var queryingSource = Source.StockIntangible.Get<IArchitectureQuerying>(Source.Context))
            {
                var sourcesTables = await queryingSource.GetTablesAsync(Source.Database, Source.Context);
                if (sourcesTables != null)
                {
                    var sourceTableQuerying = sourcesTables.AsQueryable();
                    if (TableSelector != null)
                    {
                        sourceTableQuerying = sourceTableQuerying.Where(x => TableSelector.IsAccept(Source, x));
                    }
                    return sourceTableQuerying;
                }
            }
            return Enumerable.Empty<string>();
        }

        protected virtual async Task ClearTablesAsync(DbConnection connection,ISQLDatabaseInfo info,IEnumerable<string> tables)
        {
            foreach (var item in tables)
            {
                var query = new Query().FromRaw(SQLCognateMirrorCopy.GetFullName(new SQLTableInfo(info.Database, item), info.Compiler))
                    .AsDelete();
                var sql = info.Compiler.Compile(query).ToString();
                await connection.ExecuteNoQueryAsync(sql);
            }
        }

        protected virtual async Task CopyAsync(DbConnection sourceConn, DbConnection destConn,IEnumerable<string> tables,CancellationToken token)
        {
            foreach (var item in tables)
            {
                token.ThrowIfCancellationRequested();
                using (var sourceCommand = sourceConn.CreateCommand())
                {
                    sourceCommand.CommandText = Source.CreateQuerySql(item);
                    using (var reader = sourceCommand.ExecuteReader())
                    {
                        var cp = new SQLMirrorCopy(new DefaultRowDataReader(reader, new QueryTranslateResult(sourceCommand.CommandText)),
                            new SQLMirrorTarget(destConn, new SQLTableInfo(Destination.Database, item)),
                            Destination.Compiler,
                            BatchSize);
                        await cp.CopyAsync(Source.Context);
                    }
                }
            }

        }
    }
}
