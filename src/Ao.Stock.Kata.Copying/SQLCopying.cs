using Ao.Stock.Mirror;
using DatabaseSchemaReader.Compare;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;

namespace Ao.Stock.Kata.Copying
{
    public class SQLCopying: SQLCopyingBase
    {
        public SQLCopying(ISQLDatabaseInfo source, ISQLDatabaseInfo destination) : base(source, destination)
        {
        }

        public bool SynchronousStructure { get; set; } = true;

        public bool WithDelete { get; set; } = true;

        public IEnumerable<string>? TableFilter { get; set; }

        public SqlSyncTypes SyncType { get; set; } = SqlSyncTypes.Tables;

        protected override async Task OnRunningAsync(CancellationToken token = default)
        {
            if (SynchronousStructure)
            {
                await SynchronousStructureAsync(token);
            }
            if (WithDelete)
            {
                var tables = await GetTablesAsync(token);
                foreach (var table in tables) 
                {
                    var sql = GenerateDeleteSql(Destination, table);
                    await Destination.DbConnection.ExecuteNoQueryAsync(sql, token: token);
                }
            }
        }

        protected override Task<IEnumerable<string>> GetTablesAsync(CancellationToken token = default)
        {
            var allSource = new DatabaseReader(Source.DbConnection) 
            { 
                Owner=Source.Database,
            }.TablesQuickView();
            var query = allSource.Select(x => x.Name);
            if (TableFilter!=null)
            {
                query = query.Where(x => TableFilter.Contains(x));
            }
            return Task.FromResult<IEnumerable<string>>(query.ToList());
        }
        protected void ReadSync(DatabaseReader reader)
        {
            if ((SyncType& SqlSyncTypes.Tables)!=0)
            {
                reader.AllTables();
            }
            if ((SyncType & SqlSyncTypes.StoredProcedures) != 0)
            {
                reader.AllStoredProcedures();
            }
            if ((SyncType & SqlSyncTypes.Views) != 0)
            {
                reader.AllViews();
            }
            if ((SyncType & SqlSyncTypes.Users) != 0)
            {
                reader.AllUsers();
            }
            if ((SyncType & SqlSyncTypes.Schemas) != 0)
            {
                reader.AllSchemas();
            }
            DatabaseSchemaFixer.UpdateReferences(reader.DatabaseSchema);
        }
        
        public virtual async Task SynchronousStructureAsync(CancellationToken token = default)
        {
            var sourceReader = new DatabaseReader(Source.DbConnection) { Owner = Source.Database };
            var destReader = new DatabaseReader(Destination.DbConnection) { Owner = Destination.Database };
            ReadSync(sourceReader);
            ReadSync(destReader);
            if (TableFilter != null)
            {
                sourceReader.DatabaseSchema.Tables.RemoveAll(x => !TableFilter.Contains(x.Name));
            }
            foreach (var item in sourceReader.DatabaseSchema.Tables)
            {
                item.SchemaOwner = Destination.Database;
            }
            var mig = new CompareSchemas(destReader.DatabaseSchema, sourceReader.DatabaseSchema);
            var script = mig.Execute();
            if (string.IsNullOrEmpty(script))
            {
                return;
            }
            using (var comm = Destination.DbConnection.CreateCommand(script))
            {
                await comm.ExecuteNonQueryAsync(token);
            }
        }
    }
    [Flags]
    public enum SqlSyncTypes
    {
        Tables = 1,
        StoredProcedures = Tables << 1,
        Views = Tables << 2,
        Users = Tables << 3,
        Schemas = Tables << 4,
        All = Tables | StoredProcedures | Views | Users | Schemas,
    }
}
