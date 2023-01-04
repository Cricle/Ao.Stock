using SqlKata.Compilers;

namespace Ao.Stock.Kata.Copying
{
    public interface ISQLDatabaseInfo
    {
        string Database { get; }

        IIntangibleContext Context { get; }

        IStockIntangible StockIntangible { get; }

        Compiler Compiler { get; }

        string CreateQuerySql(string tableName);
    }
}
