using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public interface IAsyncConverter<TInput, TOutput>
    {
        Task<TOutput> ConvertAsync(TInput input, CancellationToken token = default);
    }
}
