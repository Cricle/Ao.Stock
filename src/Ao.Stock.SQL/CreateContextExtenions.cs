namespace Ao.Stock.SQL
{
    public static class CreateContextExtenions
    {
        public static IIntangibleContext CreateContext(this SQLStockIntangible stock, IIntangibleContext connContext)
        {
            return new IntangibleContext
            {
                [SQLStockIntangible.IntangibleProviderKey] = stock,
                [SQLStockIntangible.SQLContextKey] = connContext
            };
        }
        public static IIntangibleContext CreateContext(this SQLStockIntangible stock,string connStr)
        {
            return new IntangibleContext
            {
                [SQLStockIntangible.IntangibleProviderKey] = stock,
                [SQLStockIntangible.ConnectionStringKey] = connStr
            };
        }
    }
}
