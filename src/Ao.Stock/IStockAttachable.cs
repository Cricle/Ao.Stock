using System.Collections.Generic;

namespace Ao.Stock
{
    public interface IStockAttachable
    {
        IReadOnlyList<IStockAttack>? Attacks { get; }
    }
}
