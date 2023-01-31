﻿namespace Ao.Stock.SQL.PostgreSql
{
    public class PostgreSqlAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public PostgreSqlAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, PostgreSqlStockIntangible.Default)
        {
        }
        public PostgreSqlAutoMigrateRunner(string connectionString, IStockType newStockType)
            : base(connectionString, newStockType, PostgreSqlStockIntangible.Default)
        {
        }
    }
}
