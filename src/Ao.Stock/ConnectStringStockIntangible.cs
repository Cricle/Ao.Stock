namespace Ao.Stock
{
    public class ConnectStringStockIntangible : IStockIntangible
    {
        public const string SQLIntangibleContextKey = "SQLIntangibleContext";

        public static readonly ConnectStringStockIntangible Instance = new ConnectStringStockIntangible();

        private ConnectStringStockIntangible() { }

        public void Config<T>(ref T input, IIntangibleContext? context)
        {
        }

        public virtual T Get<T>(IIntangibleContext? context)
        {
            if (typeof(T) == typeof(ConnectionStringBox))
            {
                var ctx = context.GetOrDefault<IIntangibleContext>(SQLIntangibleContextKey) ?? context;
                return (T)(object)new ConnectionStringBox(ctx?.ToString(), ctx);
            }
            return default;
        }
    }
}
