using System;

namespace Ao.Stock
{
    public static class CastHelperCastExtensions
    {
        public static object? Convert(this ICastHelper helper, object input, Type outputType, bool throwException = false)
        {
            if (helper.TryConvert(input, outputType, out var output, out var ex))
            {
                return output;
            }
            if (throwException)
            {
                throw ex;
            }
            return null;
        }
    }
}
