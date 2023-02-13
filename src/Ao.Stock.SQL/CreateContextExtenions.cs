using System.Data.Common;

namespace Ao.Stock.SQL
{
    public static class CreateContextExtenions
    {
        public static DbConnection GetDbContext(this IStockIntangible stock, string connStr)
        {
            return stock.Get<DbConnection>(connStr);
        }
        public static DbConnection GetDbConneciton(this IStockIntangible stock, IIntangibleContext connContext)
        {
            return stock.Get<DbConnection>(connContext);
        }
        public static T Get<T>(this IStockIntangible stock, IIntangibleContext connContext)
        {
            var ctx = CreateContext(stock, connContext);

            return stock.Get<T>(ctx);
        }
        public static T Get<T>(this IStockIntangible stock, string connStr)
        {
            var ctx = CreateContext(stock, connStr);

            return stock.Get<T>(ctx);
        }
        public static IIntangibleContext CreateContext(this IStockIntangible stock, IIntangibleContext connContext)
        {
            return new IntangibleContext
            {
                [SQLStockIntangible.IntangibleProviderKey] = stock,
                [SQLStockIntangible.SQLContextKey] = connContext
            };
        }
        public static IIntangibleContext CreateContext(this IStockIntangible stock,string connStr)
        {
            return new IntangibleContext
            {
                [SQLStockIntangible.IntangibleProviderKey] = stock,
                [SQLStockIntangible.ConnectionStringKey] = connStr
            };
        }
    }
}
