using Ao.Stock.Querying;
using SqlKata;
using System;

namespace Ao.Stock.Kata
{
    public class KataWhereMetadata : QueryMetadata
    {
        public KataWhereMetadata(Query query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        public Query Query { get; }
    }
}
