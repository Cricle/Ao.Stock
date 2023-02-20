using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ao.Stock
{

    public class StockEnvironment : List<IStockIntangible>, IStockEnvironment
    {
        public StockEnvironment()
        {
        }

        public StockEnvironment(IEnumerable<IStockIntangible> collection) : base(collection)
        {
        }

        public StockEnvironment(int capacity) : base(capacity)
        {
        }

        private IStockIntangible? currentIntangible;
        private string? enableCode;

        public string? EngineCode => enableCode;

        public IStockIntangible? CurrentIntangible
        {
            get => currentIntangible;
            set
            {
                enableCode = (value as IEngineCodeProvider)?.EngineCode;
                currentIntangible = value;
            }
        }
    }
}
