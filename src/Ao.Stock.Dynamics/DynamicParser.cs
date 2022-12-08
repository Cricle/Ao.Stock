using Ao.Stock.Querying;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Ao.Stock.Dynamics
{
    public class Visitor
    {
        public void Merge(IQueryable query, IQueryMetadata metadata)
        {
            if (metadata is GroupMetadata group)
            {
                query.GroupBy($"new ({group.Target})");
            }
        }
    }
}
