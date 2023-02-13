using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public class DelegateAsyncConverter<TOutput> : IAsyncConverter<DbDataReader, List<TOutput>>
    {
        public DelegateAsyncConverter(Func<DbDataReader, CancellationToken, TOutput> converter)
        {
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public Func<DbDataReader,CancellationToken, TOutput> Converter { get; }

        public Task<List<TOutput>> ConvertAsync(DbDataReader input, CancellationToken token = default)
        {
            var hasToken = token != default;
            var res = new List<TOutput>();
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
                res.Add(Converter(input, token));
            }
            return Task.FromResult(res);
        }
    }
}
