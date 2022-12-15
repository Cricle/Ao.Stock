using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock
{
    public interface IStockType : INamed, IStockAttachable, IStockTyped
    {
        IReadOnlyList<IStockProperty>? Properties { get; }
    }
}
