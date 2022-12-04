using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public interface IStockType : INamed
    {
        Type? Type { get; }

        IReadOnlyList<IStockProperty>? Properties { get; }

        IReadOnlyList<IStockAttack>? Attacks { get; }
    }
}
