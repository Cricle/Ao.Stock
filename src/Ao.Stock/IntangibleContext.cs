using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ao.Stock
{
    public class IntangibleContext : Dictionary<object, object>, IIntangibleContext
    {
        public IntangibleContext()
        {
        }

        public IntangibleContext(IDictionary<object, object> dictionary) : base(dictionary)
        {
        }

        public IntangibleContext(IEqualityComparer<object> comparer) : base(comparer)
        {
        }

        public IntangibleContext(int capacity) : base(capacity)
        {
        }

        public IntangibleContext(IDictionary<object, object> dictionary, IEqualityComparer<object> comparer) : base(dictionary, comparer)
        {
        }

        public IntangibleContext(int capacity, IEqualityComparer<object> comparer) : base(capacity, comparer)
        {
        }

        protected IntangibleContext(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
