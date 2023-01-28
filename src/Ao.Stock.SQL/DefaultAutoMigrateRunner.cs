using Ao.Stock.Comparering;

namespace Ao.Stock.SQL
{
    public class DefaultAutoMigrateRunner : IAutoMigrateRunner
    {
        public DefaultAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName, IStockIntangible stockIntangible)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            NewStockType = newStockType ?? throw new ArgumentNullException(nameof(newStockType));
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            StockIntangible = stockIntangible ?? throw new ArgumentNullException(nameof(stockIntangible));
        }

        public string ConnectionString { get; }

        public IStockType NewStockType { get; }

        public string TableName { get; }

        public Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? Project { get; set; }

        public IStockIntangible StockIntangible { get; }

        public AutoMigrationHelper? GetAutoMigrationHelper()
        {
            return StockIntangible.Get<AutoMigrationHelper>(new IntangibleContext
            {
                [SQLStockIntangible.ConnectionStringKey] = ConnectionString
            });
        }
        public void Migrate()
        {
            using (var auto = GetAutoMigrationHelper())
            {
                auto.EnsureDatabaseCreated();
                auto.Begin(NewStockType)
                    .ScaffoldCompareAndMigrate(TableName, Project);
            }
        }
    }
}
