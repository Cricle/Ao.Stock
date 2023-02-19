using System.Collections.Generic;

namespace Ao.Stock.Querying
{
    public static class SqlMethodTranslatorHelpers<TContext>
    {
        public static DefaultMethodTranslator<TContext> Mysql(IMethodWrapper<TContext>? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.Mysql(), wrapper ?? DefaultMethodWrapper<TContext>.MySql);
        }
        public static DefaultMethodTranslator<TContext> MariaDB(IMethodWrapper<TContext>? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.MariaDB(), wrapper ?? DefaultMethodWrapper<TContext>.MySql);
        }
        public static DefaultMethodTranslator<TContext> Sqlite(IMethodWrapper<TContext>? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.Sqlite(), wrapper ?? DefaultMethodWrapper<TContext>.Sqlite);
        }
        public static DefaultMethodTranslator<TContext> SqlServer(IMethodWrapper<TContext>? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.SqlServer(), wrapper ?? DefaultMethodWrapper<TContext>.SqlServer);
        }
        public static DefaultMethodTranslator<TContext> PostgrSql(IMethodWrapper<TContext>? wrapper = null)
        {
            return new DefaultMethodTranslator<TContext>(SqlMethodProvider.PostgrSql(), wrapper ?? DefaultMethodWrapper<TContext>.Postgres);
        }
    }

}
