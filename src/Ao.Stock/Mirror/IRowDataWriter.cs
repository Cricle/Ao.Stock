using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public interface IRowDataWriter : IRowDataWriter<string>
    {

    }
    public interface IRowDataWriter<TKey> : IDisposable
    {
        RowWriteResult<TKey> Write(IEnumerable<TKey> names, IEnumerable<IEnumerable<object>> values);

        Task<RowWriteResult<TKey>> WriteAsync(IEnumerable<TKey> names, IEnumerable<IEnumerable<object>> values);

        IRowAnonymousDataWriter<TKey> CreateAnonymous(IEnumerable<TKey> names);
    }
}
