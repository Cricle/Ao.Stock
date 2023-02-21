using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public interface IMirrorCopy<TResult>
    {
        Task<IList<TResult>> CopyAsync();
    }
}
