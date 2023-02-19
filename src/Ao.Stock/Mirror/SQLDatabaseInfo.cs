using Ao.Stock.Querying;
using System;

namespace Ao.Stock.Mirror
{
    public class DelegateSQLDatabaseInfo : SQLDatabaseInfo
    {
        public DelegateSQLDatabaseInfo(string database,
            IIntangibleContext context,
            IStockIntangible stockIntangible,
            IMethodWrapper methodWrapper)
            : this(database, context, stockIntangible, methodWrapper, DefaultCompileSql)
        {
        }
        public DelegateSQLDatabaseInfo(string database,
            IIntangibleContext context,
            IStockIntangible stockIntangible,
            IMethodWrapper methodWrapper,
            Func<DelegateSQLDatabaseInfo, string, string> querySqlGenerator)
            : base(database, context, stockIntangible, methodWrapper)
        {
            QuerySqlGenerator = querySqlGenerator;
        }
        public Func<DelegateSQLDatabaseInfo, string, string> QuerySqlGenerator { get; }

        public override string CreateQuerySql(string tableName)
        {
            return QuerySqlGenerator(this, tableName);
        }

        private static string DefaultCompileSql(DelegateSQLDatabaseInfo info, string tableName)
        {
            return $"SELECT * FROM {info.CreateFullName(tableName)}";
        }
    }

    public abstract class SQLDatabaseInfo : ISQLDatabaseInfo
    {
        protected SQLDatabaseInfo(string database, IIntangibleContext context, IStockIntangible stockIntangible, IMethodWrapper methodWrapper)
        {
            Database = database;
            Context = context;
            StockIntangible = stockIntangible;
            MethodWrapper = methodWrapper;
        }

        public string Database { get; }

        public IIntangibleContext Context { get; }

        public IStockIntangible StockIntangible { get; }

        public IMethodWrapper MethodWrapper { get; }

        public virtual string CreateFullName(string tableName)
        {
            if (string.IsNullOrEmpty(Database))
            {
                return MethodWrapper.Quto(tableName);
            }
            return MethodWrapper.Quto(Database) + "." + MethodWrapper.Quto(tableName);
        }

        public abstract string CreateQuerySql(string tableName);
    }
}
