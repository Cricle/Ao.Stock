using System.Linq.Expressions;

namespace Ao.Stock.Querying
{
    public interface IExpressionTypeProvider
    {
        ExpressionType ExpressionType { get; }
    }
}
