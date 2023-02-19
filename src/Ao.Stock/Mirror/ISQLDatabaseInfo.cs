using Ao.Stock.Querying;

namespace Ao.Stock.Mirror
{
    public interface ISQLDatabaseInfo
    {
        string Database { get; }

        IIntangibleContext Context { get; }

        IStockIntangible StockIntangible { get; }

        IMethodWrapper MethodWrapper { get; }

        string CreateFullName(string tableName);

        string CreateQuerySql(string tableName);
    }
}
