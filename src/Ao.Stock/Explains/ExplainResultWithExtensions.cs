using System.Collections.Generic;
using System.Data;

namespace Ao.Stock.Explains
{
    public static class ExplainResultWithExtensions
    {
        public static void WithReader(this IExplainResult result, IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var val = reader.GetValue(i);
                result.SetValue(name, val);
            }
        }
        public static void WithMap<TKey,TValue>(this IExplainResult result, IEnumerable<KeyValuePair<TKey,TValue>> data)
        {
            foreach (var item in data)
            {
                result.SetValue(item.Key!.ToString(), item.Value);
            }
        }
    }
}
