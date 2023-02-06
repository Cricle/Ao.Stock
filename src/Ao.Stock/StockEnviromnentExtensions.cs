using System;

namespace Ao.Stock
{
    public static class StockEnviromnentExtensions
    {
        public static bool CurrentIsMysql(this IStockEnvironment enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.Mysql);
        }
        public static bool CurrentIsMariaDB(this IStockEnvironment enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.MariaDB);
        }
        public static bool CurrentIsSqlServer(this IStockEnvironment enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.SqlServer);
        }
        public static bool CurrentIsSqlite(this IStockEnvironment enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.Sqlite);
        }
        public static bool CurrentIsPostgrSql(this IStockEnvironment enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.PostgrSql);
        }
        public static bool CurrentIsOracle(this IStockEnvironment enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.Oracle);
        }

        public static bool CheckDbCode(IStockEnvironment enviromnent, string code)
        {
            return string.Equals(code, enviromnent.EngineCode, StringComparison.OrdinalIgnoreCase);
        }
    }
}
