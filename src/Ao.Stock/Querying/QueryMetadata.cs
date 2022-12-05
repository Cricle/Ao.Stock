using System.Text;

namespace Ao.Stock.Querying
{
    public class QueryMetadata : IQueryMetadata
    {
        public virtual void ToString(StringBuilder builder)
        {
            builder.Append(ToString());
        }
    }
}
