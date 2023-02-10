namespace Ao.Stock.Kata.Copying
{
    public class DelegateSQLTableSelector : ISQLTableSelector
    {
        public DelegateSQLTableSelector(Func<ISQLDatabaseInfo, string, bool> func)
        {
            Func = func;
        }

        public Func<ISQLDatabaseInfo, string, bool> Func { get; }

        public bool IsAccept(ISQLDatabaseInfo info, string tableName)
        {
            return Func(info, tableName);
        }
    }
}
