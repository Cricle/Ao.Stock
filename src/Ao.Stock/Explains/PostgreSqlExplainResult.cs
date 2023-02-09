using System.ComponentModel;

namespace Ao.Stock.Explains
{
    public class PostgreSqlExplainResult : ExplainResultBase<PostgreSqlExplainResult>
    {
        [DisplayName("QUERY PLAN")]
        public string? QueryPlan { get; set; }
    }
}
