using System;
using System.Data.Common;

namespace Ao.Stock
{
    public class DelegateSQLArchitectureQuerying : SQLArchitectureQuerying
    {
        public DelegateSQLArchitectureQuerying(DbConnection dbConnection, string getDatabasesSql, Func<string, IIntangibleContext, string> getTableSqlFunc) : base(dbConnection)
        {
            GetDatabasesSql = getDatabasesSql ?? throw new ArgumentNullException(nameof(getDatabasesSql));
            GetTableSqlFunc = getTableSqlFunc ?? throw new ArgumentNullException(nameof(getTableSqlFunc));
        }

        public string GetDatabasesSql { get; }

        public Func<string, IIntangibleContext, string> GetTableSqlFunc { get; }

        protected override DbCommand GetGetDatabasesCommand()
        {
            var comm = DbConnection.CreateCommand();
            comm.CommandText = GetDatabasesSql;
            return comm;
        }

        protected override DbCommand GetGetTablesCommand(string database, IIntangibleContext context)
        {
            var comm = DbConnection.CreateCommand();
            comm.CommandText = GetTableSqlFunc(database, context);
            return comm;
        }

        public static DelegateSQLArchitectureQuerying Mysql(DbConnection dbConnection)
        {
            return new DelegateSQLArchitectureQuerying(dbConnection,
                "SHOW DATABASES;",
                (db, _) => $"SELECT `TABLE_NAME` FROM information_schema.`TABLES` WHERE TABLE_SCHEMA = '{db}'; ");
        }
        public static DelegateSQLArchitectureQuerying MariaDB(DbConnection dbConnection)
        {
            return Mysql(dbConnection);
        }
        public static DelegateSQLArchitectureQuerying Sqlite(DbConnection dbConnection)
        {
            return new DelegateSQLArchitectureQuerying(dbConnection,
                "SELECT 'main'",
                (db, _) => @"SELECT ""name""
FROM ""sqlite_master""
WHERE ""type"" IN ('table', 'view')");
        }
        public static DelegateSQLArchitectureQuerying SqlServer(DbConnection dbConnection)
        {
            return new DelegateSQLArchitectureQuerying(dbConnection,
                "SELECT name FROM sys.databases;",
                (db, _) => $"SELECT [TABLE_NAME] FROM {db}.INFORMATION_SCHEMA.TABLES;");
        }
        public static DelegateSQLArchitectureQuerying PostgreSql(DbConnection dbConnection)
        {
            return new DelegateSQLArchitectureQuerying(dbConnection,
                "SELECT datname FROM pg_database;",
                (db, _) => "SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname != 'pg_catalog' AND schemaname != 'information_schema';");
        }
    }

}
