using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public class DictionaryReaderAsyncConverter : IAsyncConverter<IDataReader, List<IDictionary<string, object>>>
    {
        private DictionaryReaderAsyncConverter() { }

        public static readonly DictionaryReaderAsyncConverter Instance = new DictionaryReaderAsyncConverter();

        public Task<List<IDictionary<string, object>>> ConvertAsync(IDataReader input, CancellationToken token = default)
        {
            var hasToken = token != default;
            var res = new List<IDictionary<string, object>>();
            while (input.Read())
            {
                if (hasToken)
                {
                    token.ThrowIfCancellationRequested();
                }
                var obj = new Dictionary<string, object>();
                for (int i = 0; i < input.FieldCount; i++)
                {
                    obj[input.GetName(i)] = input.GetValue(i);
                }
                res.Add(obj);
            }
            return Task.FromResult(res);
        }
    }
}
