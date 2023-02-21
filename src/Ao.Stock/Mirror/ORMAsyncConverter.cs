using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public readonly struct ORMAsyncConverter<TOutput> : IAsyncConverter<IDataReader, List<TOutput>>
    {
        public static readonly ORMAsyncConverter<TOutput> Default = new ORMAsyncConverter<TOutput>();

        public Task<List<TOutput>> ConvertAsync(IDataReader input, CancellationToken token = default)
        {
            var res = new List<TOutput>();
            if (token != default)
            {
                while (input.Read())
                {
                    token.ThrowIfCancellationRequested();
                    res.Add(ObjectMapper<TOutput>.Fill(input));
                }
            }
            else
            {
                while (input.Read())
                {
                    res.Add(ObjectMapper<TOutput>.Fill(input));
                }
            }

            return Task.FromResult(res);
        }
    }
}
