namespace Ao.Stock.SQL.Oracle
{
    public class OracleAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public OracleAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, OracleStockIntangible.Default)
        {
        }
        public OracleAutoMigrateRunner(string connectionString, IStockType newStockType)
            : base(connectionString, newStockType, OracleStockIntangible.Default)
        {
        }
    }
}
