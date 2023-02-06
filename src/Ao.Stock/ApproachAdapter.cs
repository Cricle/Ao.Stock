using System;

namespace Ao.Stock
{
    public abstract class ApproachAdapter : IApproachAdapter,IDisposable
    {
        protected ApproachAdapter(IStockType stockType)
        {
            StockType = stockType;
        }

        public IStockType StockType { get; }

        protected virtual void OnDispose()
        {
        }

        public void Dispose()
        {
            OnDispose();
            GC.SuppressFinalize(this);
        }
    }
}
