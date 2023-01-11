using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ao.Stock.Mirror
{
    public class FlatArray<T> : IEnumerable<IEnumerable<T>>, IDisposable
    {
        public FlatArray(int separate, int size)
        {
            this.separate = separate;
            actualSize = separate * size;
            list = ArrayPool<T>.Shared.Rent(actualSize);
        }
        private readonly T[] list;
        private readonly int actualSize;
        private readonly int separate;
        private int index;

        public int Separate => separate;

        public T[] List => list;

        public int Index => index;

        public int ActualSize => actualSize;

        public bool IsFull => index >= actualSize;

        public int ItemCount => actualSize / separate;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetIndexValue(int row, int index)
        {
            return row * separate + index;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Visit(Action<T, int> visitAction)
        {
            var count = ItemCount;
            for (int c = 0; c < count; c++)
            {
                for (int q = 0; q < separate; q++)
                {
                    var index = c * separate + q;
                    visitAction(list[index], index);
                }
            }
        }
        public void Add(T item)
        {
            list[index++] = item;
        }
        public void Reset()
        {
            index = 0;
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return new Enumerator(list, separate, actualSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(list);
            GC.SuppressFinalize(this);
        }

        internal class Enumerator : IEnumerator<IEnumerable<T>>
        {
            public T[] List;
            private int index;
            private readonly int separate;
            private readonly int actualCount;

            public Enumerator(T[] list, int separate, int actualSize)
            {
                List = list;
                this.separate = separate;
                this.actualCount = actualSize;
                Reset();
            }

            public IEnumerable<T> Current => List.Skip(index).Take(separate);

            object IEnumerator.Current => Current;

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                if (index >= actualCount - separate)
                {
                    return false;
                }
                index += separate;
                return true;
            }

            public void Reset()
            {
                index = -separate;
            }
        }
    }
}
