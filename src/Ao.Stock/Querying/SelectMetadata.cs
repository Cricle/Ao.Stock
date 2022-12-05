using System;

namespace Ao.Stock.Querying
{
    public class SelectMetadata : QueryMetadata, IEquatable<SelectMetadata>
    {
        public SelectMetadata(IQueryMetadata target)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public IQueryMetadata Target { get; }

        public override string? ToString()
        {
            return Target.ToString();
        }

        public override int GetHashCode()
        {
            return Target.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SelectMetadata);
        }

        public bool Equals(SelectMetadata? other)
        {
            if (other==null)
            {
                return false;
            }
            return other.Target.Equals(Target);
        }
    }
}
