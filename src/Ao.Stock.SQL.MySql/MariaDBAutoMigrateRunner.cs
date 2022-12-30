namespace Ao.Stock.SQL.MySql
{
    public class MariaDBAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public MariaDBAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, MariaDBStockIntangible.Default)
        {
        }
    }
}
