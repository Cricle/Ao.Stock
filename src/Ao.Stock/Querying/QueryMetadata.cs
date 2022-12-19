using System.Collections.Generic;
using System.Text;

namespace Ao.Stock.Querying
{
    public class QueryMetadata : IQueryMetadata
    {
        public virtual IEnumerable<IQueryMetadata> GetChildren()
        {
            yield break;
        }

        public virtual void ToString(StringBuilder builder)
        {
            builder.Append(ToString());
        }
    }
}
