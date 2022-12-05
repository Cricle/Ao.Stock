using System;

namespace Ao.Stock.Querying
{
    public class GroupMetadata : QueryMetadata, IEquatable<GroupMetadata>
    {
        public GroupMetadata(IQueryMetadata target)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public IQueryMetadata Target { get; }

        public override string ToString()
        {
            return "group by " + Target;
        }
        public override int GetHashCode()
        {
            return Target.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as GroupMetadata);
        }

        public bool Equals(GroupMetadata? other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Target.Equals(Target);
        }
    }
}
