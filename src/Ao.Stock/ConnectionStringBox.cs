namespace Ao.Stock
{
    public class ConnectionStringBox
    {
        public ConnectionStringBox(string? connectionString, IIntangibleContext? source)
        {
            ConnectionString = connectionString;
            Source = source;
        }

        public string? ConnectionString { get; }

        public IIntangibleContext? Source { get; }

        public override string? ToString()
        {
            return ConnectionString;
        }
    }
}
