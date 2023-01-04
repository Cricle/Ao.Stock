using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ao.Stock.Mirror
{
    public abstract class DataMirrorCopy<TKey> : UndefinedDataMirrorCopy<TKey, RowWriteResult<TKey>, IEnumerable<IEnumerable<object?>>>
    {
        protected DataMirrorCopy(IRowDataReader dataReader) : base(dataReader)
        {
        }

        protected DataMirrorCopy(IRowDataReader dataReader, int batchSize) : base(dataReader, batchSize)
        {
        }

        protected override IEnumerable<IEnumerable<object?>> ConvertToInput(FlatArray<object?> arr, int? size, IIntangibleContext context)
        {
            if (size == null)
            {
                return arr;
            }
            return arr.Take(size.Value);
        }
    }
}
