namespace Ao.Stock.SQL.MySql
{
    public class MySqlAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public MySqlAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, MySqlStockIntangible.Default)
        {
        }
    }
}
