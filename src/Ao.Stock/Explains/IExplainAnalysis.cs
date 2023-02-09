namespace Ao.Stock.Explains
{
    public interface IExplainAnalysis<TInput, TResult>
        where TResult: IExplainResult
    {
        TResult Analyze(TInput input);
    }
}
