using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Stock.Querying
{
    public class SortMetadata : QueryMetadata,IEquatable<SortMetadata>, IQueryMetadata
    {
        public SortMetadata(SortMode sortMode, IQueryMetadata target)
        {
            SortMode = sortMode;
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public SortMode SortMode { get; }

        public IQueryMetadata Target { get; }

        public override IEnumerable<IQueryMetadata> GetChildren()
        {
            yield return Target;
        }

        public override string ToString()
        {
            return $"order by {Target} {SortMode}";
        }

        public bool Equals(SortMetadata? other)
        {
            if (other==null)
            {
                return false;   
            }
            return other.SortMode==SortMode&&
                other.Target.Equals(Target);
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as SortMetadata);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SortMode, Target);
        }
    }
}
