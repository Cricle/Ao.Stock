namespace Ao.Stock.Querying
{
    public static class SqlMethodTranslatorHelpers<TContext>
    {
        public static DefaultMethodTranslator<TContext> Mysql(IMethodWrapper? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.Mysql(), wrapper ?? DefaultMethodWrapper.MySql);
        }
        public static DefaultMethodTranslator<TContext> MariaDB(IMethodWrapper? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.MariaDB(), wrapper ?? DefaultMethodWrapper.MySql);
        }
        public static DefaultMethodTranslator<TContext> Sqlite(IMethodWrapper? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.Sqlite(), wrapper ?? DefaultMethodWrapper.Sqlite);
        }
        public static DefaultMethodTranslator<TContext> SqlServer(IMethodWrapper? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.SqlServer(), wrapper ?? DefaultMethodWrapper.SqlServer);
        }
        public static DefaultMethodTranslator<TContext> PostgrSql(IMethodWrapper? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.PostgrSql(), wrapper ?? DefaultMethodWrapper.PostgreSql);
        }
    }

}
