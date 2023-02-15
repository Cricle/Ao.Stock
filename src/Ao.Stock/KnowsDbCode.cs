using System;

namespace Ao.Stock
{
    public static class KnowsDbCode
    {
        public const string Mysql = "mysql";
        public const string MariaDB = "mariadb";
        public const string SqlServer = "sqlserver";
        public const string Sqlite = "sqlite";
        public const string PostgreSql = "postgresql";
        public const string Oracle = "oracle";

        public static bool IsMysql(string type)
        {
            return NoCastEquals(type, Mysql);
        }
        public static bool IsMariaDB(string type)
        {
            return NoCastEquals(type, MariaDB);
        }
        public static bool IsSqlServer(string type)
        {
            return NoCastEquals(type, SqlServer);
        }
        public static bool IsSqlite(string type)
        {
            return NoCastEquals(type, Sqlite);
        }
        public static bool IsPostgreSql(string type)
        {
            return NoCastEquals(type, PostgreSql);
        }
        public static bool IsOracle(string type)
        {
            return NoCastEquals(type, Oracle);
        }

        private static bool NoCastEquals(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

    }
}
