using Ao.Stock.Querying;
using System;
using System.Data.Common;

namespace Ao.Stock.Mirror
{
    public class DelegateSQLDatabaseInfo : SQLDatabaseInfo
    {
        public DelegateSQLDatabaseInfo(string database,
            DbConnection dbConnection,
            IMethodWrapper methodWrapper)
            : this(database, dbConnection, methodWrapper, DefaultCompileSql)
        {
        }
        public DelegateSQLDatabaseInfo(string database,
            DbConnection dbConnection,
            IMethodWrapper methodWrapper,
            Func<DelegateSQLDatabaseInfo, string, string> querySqlGenerator)
            : base(database, dbConnection, methodWrapper)
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
}
