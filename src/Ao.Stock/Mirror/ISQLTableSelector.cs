namespace Ao.Stock.Mirror
{
    public interface ISQLTableSelector
    {
        bool IsAccept(ISQLDatabaseInfo info, string tableName);
    }
}
