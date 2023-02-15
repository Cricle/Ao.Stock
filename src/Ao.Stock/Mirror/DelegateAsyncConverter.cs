using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public readonly struct DelegateAsyncConverter<TOutput> : IAsyncConverter<DbDataReader, List<TOutput>>
    {
        public DelegateAsyncConverter(Func<DbDataReader, TOutput> converter)
        {
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public Func<DbDataReader, TOutput> Converter { get; }

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
                res.Add(Converter(input));
            }
            return Task.FromResult(res);
        }
    }
}
