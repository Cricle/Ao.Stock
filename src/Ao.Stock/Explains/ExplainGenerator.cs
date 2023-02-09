namespace Ao.Stock.Explains
{
    public static class ExplainGenerator
    {
        public static string MySql(string sql)
        {
            return "EXPLAIN " + sql;
        }
        public static string SqlServer(string sql)
        {
            if (!sql.EndsWith(";"))
            {
                sql += ";";
            }
            return "SET SHOWPLAN_ALL ON;\nGO;\n" + sql + "\nSET SHOWPLAN_ALL OFF;\nGO;";
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
