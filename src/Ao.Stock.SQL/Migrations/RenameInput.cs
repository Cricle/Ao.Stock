namespace Ao.Stock.SQL.Migrations
{
    public class RenameInput : IEquatable<RenameInput>
    {
        public RenameInput(string table, string oldName, string newName)
        {
            Table = table ?? throw new ArgumentNullException(nameof(table));
            OldName = oldName ?? throw new ArgumentNullException(nameof(oldName));
            NewName = newName ?? throw new ArgumentNullException(nameof(newName));
        }

        public string Table { get; }

        public string OldName { get; }

        public string NewName { get; }

        public bool Equals(RenameInput? other)
        {
            if (other ==null)
            {
                return false;
            }
            return other.Table == Table &&
                other.OldName == OldName && other.NewName == NewName;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Table, OldName, NewName);
        }
        public override string ToString()
        {
            return $"{{Table:{Table}, {OldName}->{NewName}}}";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RenameInput);
        }
    }
}
