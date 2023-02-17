using Microsoft.Extensions.ObjectPool;
using System.Data;
using System.Data.Common;

namespace Ao.Stock.LinkPool
{
    public class DbConnectionPool : StockObjectPool<DbConnection>
    {
        public DbConnectionPool(IStockIntangible stockIntangible, IIntangibleContext context)
            : base(stockIntangible, context)
        {
        }

        public DbConnectionPool(IPooledObjectPolicy<DbConnection> policy, IStockIntangible stockIntangible, IIntangibleContext context)
            : base(policy, stockIntangible, context)
        {
        }

        public DbConnectionPool(ObjectPoolProvider provider, IStockIntangible stockIntangible, IIntangibleContext context)
            : base(provider, stockIntangible, context)
        {
        }

        public DbConnectionPool(ObjectPoolProvider provider, IPooledObjectPolicy<DbConnection> policy, IStockIntangible stockIntangible, IIntangibleContext context)
            : base(provider, policy, stockIntangible, context)
        {
        }
        class PooledObjectPolicy : IPooledObjectPolicy<DbConnection>
        {
            public PooledObjectPolicy(IStockIntangible stockIntangible, IIntangibleContext context)
            {
                StockIntangible = stockIntangible;
                Context = context;
            }

            public IStockIntangible StockIntangible { get; }

            public IIntangibleContext Context { get; }

            public DbConnection Create()
            {
                return StockIntangible.Get<DbConnection>(Context);
            }

            public bool Return(DbConnection obj)
            {
                if (obj.State != ConnectionState.Open)
                {
                    obj.Dispose();
                    return false;
                }
                return true;
            }
        }
    }
}
