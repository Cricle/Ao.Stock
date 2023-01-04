namespace Ao.Stock.Kata.Copying
{
    public interface ISQLTableSelector
    {
        bool IsAccept(ISQLDatabaseInfo info, string tableName);
    }
}
