using Ao.Stock.LinkPool;
using Microsoft.Extensions.ObjectPool;
using System.Data.Common;

namespace Ao.Stock
{
    public static class StockPoolExtensions
    {
        public static DbConnectionPool CreateDbConnectionPool(this IStockIntangible stock, IIntangibleContext context)
        {
            return new DbConnectionPool(stock, context);
        }
        public static DbConnectionPool CreateDbConnectionPool(this IStockIntangible stock, IIntangibleContext context, IPooledObjectPolicy<DbConnection> policy)
        {
            return new DbConnectionPool(policy, stock, context);
        }
        public static DbConnectionPool CreateDbConnectionPool(this IStockIntangible stock, IIntangibleContext context, ObjectPoolProvider provider)
        {
            return new DbConnectionPool(provider, stock, context);
        }
        public static DbConnectionPool CreateDbConnectionPool(this IStockIntangible stock, IIntangibleContext context, int maximumRetained)
        {
            return new DbConnectionPool(new DefaultObjectPoolProvider { MaximumRetained = maximumRetained }, stock, context);
        }
        public static DbConnectionPool CreateDbConnectionPool(this IStockIntangible stock, IIntangibleContext context, IPooledObjectPolicy<DbConnection> policy, ObjectPoolProvider provider)
        {
            return new DbConnectionPool(provider, policy, stock, context);
        }
    }
}
