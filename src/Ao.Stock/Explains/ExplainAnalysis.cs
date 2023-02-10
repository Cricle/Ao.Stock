using System.Data;

namespace Ao.Stock.Explains
{
    public class ExplainAnalysis<TResult> : IExplainAnalysis<IDataReader, TResult>
          where TResult : IExplainResult, new()
    {
        public static readonly ExplainAnalysis<TResult> Default = new ExplainAnalysis<TResult>();

        public TResult Analyze(IDataReader input)
        {
            var result = new TResult();
            result.WithReader(input);
            return result;
        }
    }
}
