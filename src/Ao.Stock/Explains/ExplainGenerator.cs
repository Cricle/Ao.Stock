namespace Ao.Stock.Explains
{
    public static class ExplainGenerator
    {
        public static readonly string SqlServerStart = "SET SHOWPLAN_ALL ON;";
        public static readonly string SqlServerEnd = "SET SHOWPLAN_ALL ON;";

        public static string MySql(string sql)
        {
            return "EXPLAIN " + sql;
        }
        public static string PostgreSql(string sql)
        {
            return "EXPLAIN " + sql;
        }
        public static string Sqlite(string sql)
        {
            return "EXPLAIN " + sql;
        }
    }
}
