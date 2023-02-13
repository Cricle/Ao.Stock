using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public class IntReaderAsyncConverter : IAsyncConverter<DbDataReader, int>
    {
        private IntReaderAsyncConverter() { }

        public static readonly IntReaderAsyncConverter Instance = new IntReaderAsyncConverter();

        public Task<int> ConvertAsync(DbDataReader input, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            input.Read();
            return Task.FromResult(input.GetInt32(0));
        }
    }
}
