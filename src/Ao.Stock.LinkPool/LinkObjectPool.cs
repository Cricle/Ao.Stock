using Microsoft.Extensions.ObjectPool;
using System;

namespace Ao.Stock.LinkPool
{
    public class LinkObjectPool<T> : ObjectPool<T>, IDisposable
        where T : class
    {
        protected static readonly DefaultObjectPoolProvider defaultProvider = new DefaultObjectPoolProvider();

        public LinkObjectPool(IPooledObjectPolicy<T> policy)
            : this(defaultProvider, policy)
        {
        }

        public LinkObjectPool(ObjectPoolProvider provider, IPooledObjectPolicy<T> policy)
        {
            if (policy is null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Impl = Provider.Create(policy);
        }

        public ObjectPool<T> Impl { get; }

        public ObjectPoolProvider Provider { get; }

        public void Dispose()
        {
            if (Impl is IDisposable disposable)
            {
                disposable.Dispose();
            }
            OnDisposed();
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDisposed()
        {

        }

        public override T Get()
        {
            return Impl.Get();
        }

        public override void Return(T obj)
        {
            Impl.Return(obj);
        }
    }
}
