using System.Collections.Generic;

namespace Ao.Stock
{
    public interface IStockEnvironment : IEnumerable<IStockIntangible>
    {
        string? EngineCode { get; }

        IStockIntangible? CurrentIntangible { get; set; }
    }
}
