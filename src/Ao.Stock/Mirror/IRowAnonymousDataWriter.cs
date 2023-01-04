using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public interface IRowAnonymousDataWriter<TKey> : IDisposable
    {
        RowWriteResult<TKey> Write(IEnumerable<IEnumerable<object>> values);

        Task<RowWriteResult<TKey>> WriteAsync(IEnumerable<IEnumerable<object>> values);
    }
}
