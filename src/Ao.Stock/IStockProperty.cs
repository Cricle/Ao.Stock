using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public interface IStockProperty : INamed
    {
        Type? Type { get; }

        IReadOnlyList<IStockAttack>? Attacks { get; }
    }
}
