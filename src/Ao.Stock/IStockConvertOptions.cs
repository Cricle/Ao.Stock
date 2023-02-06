using System;

namespace Ao.Stock
{
    public interface IStockConvertOptions
    {
        IStockProperty? FindProperty(IStockType type, string name);

        object? FailCast(object? input, Exception? exception);
    }
}
