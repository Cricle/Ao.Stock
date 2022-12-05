using Ao.Stock.Querying;
using SqlKata;
using System;

namespace Ao.Stock.Kata
{
    public class KataSelectMetadata : QueryMetadata
    {
        public KataSelectMetadata(Query query, string alias)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Alias = alias ?? throw new ArgumentNullException(nameof(alias));
        }

        public Query Query { get; }

        public string Alias { get; }
    }
}
