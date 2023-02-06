using System;

namespace Ao.Stock
{
    public interface ICastHelper
    {
        bool TryConvert(object? input, Type outputType, out object? output, out Exception? exception);
    }
}
