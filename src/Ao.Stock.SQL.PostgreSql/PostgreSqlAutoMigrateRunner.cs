namespace Ao.Stock.SQL.PostgreSql
{
    public class PostgreSqlAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public PostgreSqlAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, PostgreSqlStockIntangible.Default)
        {
            Project = NoRenameProject;

        }
        public PostgreSqlAutoMigrateRunner(string connectionString, IStockType newStockType)
            : base(connectionString, newStockType, PostgreSqlStockIntangible.Default)
        {
            Project = NoRenameProject;

        }
    }
}
