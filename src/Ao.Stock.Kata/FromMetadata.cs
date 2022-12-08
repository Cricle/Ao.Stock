using Ao.Stock.Querying;
using System;

namespace Ao.Stock.Kata
{
    public class FromMetadata : QueryMetadata,IEquatable<FromMetadata>
    {
        public FromMetadata(IQueryMetadata from)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
        }

        public IQueryMetadata From { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as FromMetadata);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(From);
        }

        public bool Equals(FromMetadata other)
        {
            if (other==null)
            {
                return false;
            }
            return other.From.Equals(From);
        }
        public override string ToString()
        {
            return From.ToString();
        }
    }
}
