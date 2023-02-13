using Microsoft.Extensions.ObjectPool;

namespace Ao.Stock.LinkPool
{
    public class StockObjectPool<T> : LinkObjectPool<T>
        where T : class
    {
        public StockObjectPool(IStockIntangible stockIntangible, IIntangibleContext context)
            : base(new PooledObjectPolicy(stockIntangible, context))
        {
            StockIntangible = stockIntangible;
            Context = context;
        }
        public StockObjectPool(IPooledObjectPolicy<T> policy, IStockIntangible stockIntangible, IIntangibleContext context)
            : base(policy)
        {
            StockIntangible = stockIntangible;
            Context = context;
        }
        public StockObjectPool(ObjectPoolProvider provider, IStockIntangible stockIntangible, IIntangibleContext context)
            : base(provider, new PooledObjectPolicy(stockIntangible, context))
        {
            StockIntangible = stockIntangible;
            Context = context;
        }

        public StockObjectPool(ObjectPoolProvider provider, IPooledObjectPolicy<T> policy, IStockIntangible stockIntangible, IIntangibleContext context)
            : base(provider, policy)
        {
            StockIntangible = stockIntangible;
            Context = context;
        }

        public IStockIntangible StockIntangible { get; }

        public IIntangibleContext Context { get; }

        class PooledObjectPolicy : IPooledObjectPolicy<T>
        {
            public PooledObjectPolicy(IStockIntangible stockIntangible, IIntangibleContext context)
            {
                StockIntangible = stockIntangible;
                Context = context;
            }

            public IStockIntangible StockIntangible { get; }

            public IIntangibleContext Context { get; }

            public T Create()
            {
                return StockIntangible.Get<T>(Context);
            }

            public bool Return(T obj)
            {
                return true;
            }
        }
    }
}
