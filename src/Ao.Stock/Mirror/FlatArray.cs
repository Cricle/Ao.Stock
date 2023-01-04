using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock.Mirror
{
    public class FlatArray<T> : IEnumerable<IEnumerable<T>>,IDisposable
    {
        public FlatArray(int separate, int size)
        {
            Separate = separate;
            actualSize = separate * size;
            List = ArrayPool<T>.Shared.Rent(actualSize);
        }
        private readonly int actualSize;
        private int index;

        public int Separate { get; }

        public T[] List { get; }

        public int Index => index;

        public int ActualSize => actualSize;

        public bool IsFull => index >= actualSize;

        public void Add(T item)
        {
            List[index++] = item;
        }
        public void Reset()
        {
            index = 0;
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return new Enumerator(List, Separate,actualSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(List);
            GC.SuppressFinalize(this);
        }

        internal class Enumerator : IEnumerator<IEnumerable<T>>
        {
            public T[] List;
            private int index;
            private readonly int separate;
            private readonly int actualCount;

            public Enumerator(T[] list, int separate,int actualSize)
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
