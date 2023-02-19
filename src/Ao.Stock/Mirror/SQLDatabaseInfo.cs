using Ao.Stock.Querying;
using System;
using System.Data.Common;

namespace Ao.Stock.Mirror
{

    public abstract class SQLDatabaseInfo : ISQLDatabaseInfo
    {
        protected SQLDatabaseInfo(string database, DbConnection dbConnection, IMethodWrapper methodWrapper)
        {
            Database = database;
            DbConnection = dbConnection;
            MethodWrapper = methodWrapper;
        }

        public string Database { get; }

        public DbConnection DbConnection { get; }

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
