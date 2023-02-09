using System.ComponentModel;

namespace Ao.Stock.Explains
{
    public class SqlServerExplainResult : ExplainResultBase<SqlServerExplainResult>
    {
        [DisplayName("StmtText")]
        public string? StmtText { get; set; }

        [DisplayName("StmtId")]
        public string? StmtId { get; set; }

        [DisplayName("NodeId")]
        public string? NodeId { get; set; }

        [DisplayName("Parent")]
        public string? Parent { get; set; }

        [DisplayName("PhysicalOp")]
        public string? PhysicalOp { get; set; }

        [DisplayName("LogicalOp")]
        public string? LogicalOp { get; set; }


        [DisplayName("Argument")]
        public string? Argument { get; set; }

        [DisplayName("DefinedValues")]
        public string? DefinedValues { get; set; }

        [DisplayName("EstimateRows")]
        public string? EstimateRows { get; set; }

        [DisplayName("EstimateIO")]
        public string? EstimateIO { get; set; }

        [DisplayName("EstimateCPU")]
        public string? EstimateCPU { get; set; }

        [DisplayName("AvgRowSize")]
        public string? AvgRowSize { get; set; }

        [DisplayName("TotalSubtreeCost")]
        public string? TotalSubtreeCost { get; set; }

        [DisplayName("OutputList")]
        public string? OutputList { get; set; }

        [DisplayName("Warnings")]
        public string? Warnings { get; set; }

        [DisplayName("Type")]
        public string? Type { get; set; }

        [DisplayName("Parallel")]
        public string? Parallel { get; set; }

        [DisplayName("EstimateExecutions")]
        public string? EstimateExecutions { get; set; }

    }
}
