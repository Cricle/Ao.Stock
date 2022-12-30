using Ao.Stock.SQL.MSSQL;

namespace Ao.Stock.SQL.MySql
{
    public class SqlServerAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public SqlServerAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, SqlServerStockIntangible.Default)
        {
        }
    }
}
