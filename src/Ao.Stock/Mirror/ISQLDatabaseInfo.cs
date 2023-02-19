using Ao.Stock.Querying;
using System.Data.Common;

namespace Ao.Stock.Mirror
{

    public interface ISQLDatabaseInfo
    {
        string Database { get; }

        DbConnection DbConnection { get; }

        IMethodWrapper MethodWrapper { get; }

        string CreateFullName(string tableName);

        string CreateQuerySql(string tableName);
    }
}
