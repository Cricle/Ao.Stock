using System.Collections.Generic;

namespace Ao.Stock.Kata.Mirror
{
    public class SQLMirrorCopyResult
    {
        public SQLMirrorCopyResult(int affectRows, string sql, IReadOnlyDictionary<string, object> bindings)
        {
            AffectRows = affectRows;
            Sql = sql;
            Bindings = bindings;
        }

        public int AffectRows { get; }

        public string Sql { get; }

        public IReadOnlyDictionary<string, object> Bindings { get; }
    }
}
