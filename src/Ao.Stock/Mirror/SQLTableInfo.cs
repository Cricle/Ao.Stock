using System;

namespace Ao.Stock.Mirror
{
    public readonly struct SQLTableInfo : IEquatable<SQLTableInfo>
    {
        public SQLTableInfo(string database, string table)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Table = table ?? throw new ArgumentNullException(nameof(table));
        }

        public string Database { get; }

        public string Table { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Database, Table);
        }

        public override bool Equals(object? obj)
        {
            if (obj is SQLTableInfo info)
            {
                return Equals(info);
            }
            return false;
        }

        public bool Equals(SQLTableInfo other)
        {
            return other.Database == Database &&
                other.Table == Table;
        }
        public override string ToString()
        {
            return $"{Database}.{Table}";
        }
    }
}
