using System;
using System.Collections.Generic;

namespace Ao.Stock.Mirror
{
    public interface ICircularTableReader<TKey, TValue>
    {
        int KeyCout { get; }

        int ValueCout { get; }

        Span<TKey> KeySpan { get; }

        Span<TValue> ValueSpan { get; }

        IEnumerable<TKey> Keys { get; }

        IEnumerable<TValue> Values { get; }
    }
}
