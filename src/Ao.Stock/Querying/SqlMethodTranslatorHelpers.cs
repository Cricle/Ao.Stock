namespace Ao.Stock.Querying
{
    public static class SqlMethodTranslatorHelpers<TContext>
    {
        public static DefaultMethodTranslator<TContext> Mysql(IMethodWrapper<TContext>? wrapper=null)
        {
            return new DefaultMethodTranslator<TContext>(KnowsMethods.Functions, wrapper??DefaultMethodWrapper<TContext>.MySql);
        }
        public static DefaultMethodTranslator<TContext> MariaDB(IMethodWrapper<TContext>? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(KnowsMethods.Functions, wrapper ?? DefaultMethodWrapper<TContext>.MySql);
        }
        public static DefaultMethodTranslator<TContext> Sqlite(IMethodWrapper<TContext>? wrapper = null)
        {
            var funcs = KnowsMethods.Functions;
            funcs[KnowsMethods.StrConcat] = KnowsMethods.AllOrAlso;
            funcs[KnowsMethods.Year] = "strftime('%Y',{1})";
            funcs[KnowsMethods.Month] = "strftime('%m',{1})";
            funcs[KnowsMethods.Day] = "strftime('%d',{1})";
            funcs[KnowsMethods.Hour] = "strftime('%H',{1})";
            funcs[KnowsMethods.Minute] = "strftime('%M',{1})";
            funcs[KnowsMethods.Second] = "strftime('%S',{1})";
            funcs[KnowsMethods.Date] = "strftime('%Y-%m-%d',{1})";
            funcs[KnowsMethods.Now] = "datetime(CURRENT_TIMESTAMP,'localtime')";
            funcs[KnowsMethods.DateFormat] = "strftime({2},{1})";
            funcs[KnowsMethods.Weak] = "strftime('%W',{1})";
            funcs[KnowsMethods.Quarter] = "COALESCE(NULLIF((SUBSTR({1}, 4, 2) - 1) / 3, 0), 4)";
            funcs[KnowsMethods.StrLen] = "LENGTH({1})";
            funcs[KnowsMethods.StrIndexOf] = "instr({1},{2})";
            return new DefaultMethodTranslator<TContext>(funcs, wrapper ?? DefaultMethodWrapper<TContext>.Sqlite);
        }
        public static DefaultMethodTranslator<TContext> SqlServer(IMethodWrapper<TContext>? wrapper = null)
        {
            var funcs = KnowsMethods.Functions;
            funcs[KnowsMethods.StrLen] = "LEN({1})";
            funcs[KnowsMethods.StrIndexOf] = "CHARINDEX({1},{2})";
            funcs[KnowsMethods.StrSub] = "SUBSTRING({1},{2},{3})";
            funcs[KnowsMethods.DateFormat] = "FORMAT({1},{2})";
            funcs[KnowsMethods.Weak] = "DATEPART(WEEK,{1})";
            funcs[KnowsMethods.Quarter] = "DATEPART(QUARTER,{1})";
            return new DefaultMethodTranslator<TContext>(funcs, wrapper ?? DefaultMethodWrapper<TContext>.SqlServer);
        }
        public static DefaultMethodTranslator<TContext> PostgrSql(IMethodWrapper<TContext>? wrapper = null)
        {
            var funcs = KnowsMethods.Functions;
            funcs[KnowsMethods.Year] = "to_char({1},'yyyy')";
            funcs[KnowsMethods.Month] = "to_char({1},'MM')";
            funcs[KnowsMethods.Day] = "to_char({1},'dd')";
            funcs[KnowsMethods.Hour] = "to_char({1},'HH')";
            funcs[KnowsMethods.Minute] = "to_char({1},'mm')";
            funcs[KnowsMethods.Second] = "to_char({1},'ss')";
            funcs[KnowsMethods.Microsecond] = "to_char({1},'yyyy-MM-dd')";
            funcs[KnowsMethods.StrLen] = "length({1})";
            funcs[KnowsMethods.StrLowercase] = "lower({1})";
            funcs[KnowsMethods.StrUppercase] = "upper({1})";
            funcs[KnowsMethods.StrIndexOf] = "strpos({1},{2})";
            funcs[KnowsMethods.StrTrim] = "trim(both ' ' from {1})";
            funcs[KnowsMethods.StrSub] = "substring({1} from {2} to {3})";
            funcs[KnowsMethods.Date] = "to_char({1},'yyyy-MM-dd')";
            funcs[KnowsMethods.Datediff] = "age(timestamp {1},timestamp {2})";
            funcs[KnowsMethods.Now] = "now()";
            funcs[KnowsMethods.DateFormat] = "to_char({1},{2})";
            funcs[KnowsMethods.Weak] = "to_char({1},'WW')";
            funcs[KnowsMethods.Quarter] = "EXTRACT (QUARTER FROM {1})";
            return new DefaultMethodTranslator<TContext>(funcs, wrapper?? DefaultMethodWrapper<TContext>.Postgres);
        }
    }

}
