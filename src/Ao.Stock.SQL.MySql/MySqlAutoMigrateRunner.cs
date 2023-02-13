namespace Ao.Stock.SQL.MySql
{
    public class MySqlAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public MySqlAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, MySqlStockIntangible.Default)
        {
            Project = NoRenameProject;
        }
        public MySqlAutoMigrateRunner(string connectionString, IStockType newStockType)
            : base(connectionString, newStockType, MySqlStockIntangible.Default)
        {
            Project = NoRenameProject;
        }
    }
}
