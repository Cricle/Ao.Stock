using System.Linq.Expressions;
using System.Text;

namespace Ao.Stock.Querying
{
    public interface IQueryMetadata
    {
        string? ToString();

        void ToString(StringBuilder builder);
    }
}
