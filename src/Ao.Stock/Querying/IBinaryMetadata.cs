namespace Ao.Stock.Querying
{
    public interface IBinaryMetadata: IExpressionTypeProvider,IQueryMetadata
    {
        IQueryMetadata Left { get; }

        IQueryMetadata Right { get; }
    }
}
