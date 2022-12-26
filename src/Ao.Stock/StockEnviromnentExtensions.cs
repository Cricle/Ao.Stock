using System;

namespace Ao.Stock
{
    public static class StockEnviromnentExtensions 
    {
        public static bool CurrentIsMysql(this IStockEnviromnent enviromnent)
        {
            return CheckDbCode(enviromnent,KnowsDbCode.Mysql);
        }
        public static bool CurrentIsSqlServer(this IStockEnviromnent enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.SqlServer);
        }
        public static bool CurrentIsSqlite(this IStockEnviromnent enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.Sqlite);
        }
        public static bool CurrentIsPostgrSql(this IStockEnviromnent enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.PostgrSql);
        }
        public static bool CurrentIsOracle(this IStockEnviromnent enviromnent)
        {
            return CheckDbCode(enviromnent, KnowsDbCode.Oracle);
        }

        private static bool CheckDbCode(IStockEnviromnent enviromnent,string code)
        {
            return string.Equals(code, enviromnent.CurrentCode, StringComparison.OrdinalIgnoreCase);
        }
    }
}
