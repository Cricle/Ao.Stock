using System.Collections.Generic;

namespace Ao.Stock
{
    public interface IStockType : INamed, IStockAttachable, IStockTyped
    {
        IReadOnlyList<IStockProperty>? Properties { get; }
    }
}
