using System;
using System.Collections.Generic;

namespace Ao.Stock.Mirror
{
    public interface ICircularTableWriter<TKey, TValue>
    {
        void AddKey(TKey key);

        void AddKey(Span<TKey> keys);

        void AddKey(IEnumerable<TKey> keys);

        void AddValue(TValue value);

        void AddValue(Span<TValue> values);

        void AddValue(IEnumerable<TValue> values);
    }
}
