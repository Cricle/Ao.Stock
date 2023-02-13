﻿namespace Ao.Stock.SQL.MSSQL
{
    public class SqlServerAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public SqlServerAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, SqlServerStockIntangible.Default)
        {
            Project = NoRenameProject;

        }

        public SqlServerAutoMigrateRunner(string connectionString, IStockType newStockType)
            : base(connectionString, newStockType, SqlServerStockIntangible.Default)
        {
            Project = NoRenameProject;

        }

    }
}
