using System;
using System.Linq;

namespace Ao.Stock
{
    public class DefaultStockConvertOptions: IStockConvertOptions
    {
        public static readonly DefaultStockConvertOptions Default = new DefaultStockConvertOptions((_, ex) => throw ex);

        public DefaultStockConvertOptions(Func<object?, Exception?, object?>? failCastFunc)
        {
            FailCastFunc = failCastFunc;
        }

        public Func<object?,Exception?,object?>? FailCastFunc { get; }

        public IStockProperty? FindProperty(IStockType type,string name)
        {
            return type.Properties?.FirstOrDefault(x => x.Name == name);
        }

        public object? FailCast(object? input,Exception? exception)
        {
            return FailCastFunc?.Invoke(input, exception);
        }
    }
}
