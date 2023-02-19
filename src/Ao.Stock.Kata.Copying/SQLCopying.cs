using Ao.Stock.Mirror;
using DatabaseSchemaReader.Compare;
using DatabaseSchemaReader;

namespace Ao.Stock.Kata.Copying
{
    public class SQLCopying: SQLCopyingBase
    {
        public SQLCopying(ISQLDatabaseInfo source, ISQLDatabaseInfo destination) : base(source, destination)
        {
        }

        public bool SynchronousStructure { get; set; } = true;

        public bool WithDelete { get; set; } = true;

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
            var allSource = new DatabaseReader(Source.DbConnection) { Owner=Source.Database}.TableList();
            return Task.FromResult<IEnumerable<string>>(allSource.Select(x => x.Name).ToList());
        }
        public virtual async Task SynchronousStructureAsync(CancellationToken token = default)
        {
            var allSource = new DatabaseReader(Source.DbConnection) { Owner= Source.Database }.ReadAll(token);
            foreach (var item in allSource.Tables)
            {
                item.SchemaOwner = Destination.Database;
            }
            var destReader = new DatabaseReader(Destination.DbConnection) { Owner = Destination.Database };
            var destAll = destReader.ReadAll(token);
            var mig = new CompareSchemas(destAll, allSource);
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

}
