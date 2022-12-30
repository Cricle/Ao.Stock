namespace Ao.Stock.Querying
{
    public interface IUnaryMetadata : IExpressionTypeProvider
    {
        IQueryMetadata Left { get; }
    }
}
