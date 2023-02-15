using SqlKata.Compilers;
using System;

namespace Ao.Stock.Kata
{
    public static class CompilerFetcher
    {
        public static readonly Compiler Mysql = new MySqlCompiler();
        public static readonly Compiler MariaDB = new MySqlCompiler();
        public static readonly Compiler SqlServer = new SqlServerCompiler();
        public static readonly Compiler Sqlite = new SqliteCompiler();
        public static readonly Compiler Oracle = new OracleCompiler();
        public static readonly Compiler PostgresSql = new PostgresCompiler();

        public static Compiler GetCompiler(string dbType)
        {
            if (KnowsDbCode.IsMysql(dbType))
            {
                return Mysql;
            }
            if (KnowsDbCode.IsMariaDB(dbType))
            {
                return MariaDB;
            }
            if (KnowsDbCode.IsSqlServer(dbType))
            {
                return SqlServer;
            }
            if (KnowsDbCode.IsSqlite(dbType))
            {
                return Sqlite;
            }
            if (KnowsDbCode.IsOracle(dbType))
            {
                return Oracle;
            }
            if (KnowsDbCode.IsPostgreSql(dbType))
            {
                return PostgresSql;
            }
            throw new NotSupportedException(dbType);
        }
    }
}
