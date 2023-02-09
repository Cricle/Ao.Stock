using System.ComponentModel;

namespace Ao.Stock.Explains
{
    public class SqliteExplainResult : ExplainResultBase<SqliteExplainResult>
    {
        [DisplayName("addr")]
        public string? Addr { get; set; }

        [DisplayName("opcode")]
        public string? OpCode { get; set; }

        [DisplayName("p1")]
        public string? P1 { get; set; }

        [DisplayName("p2")]
        public string? P2 { get; set; }

        [DisplayName("p3")]
        public string? P3 { get; set; }

        [DisplayName("p4")]
        public string? P4 { get; set; }

        [DisplayName("p5")]
        public string? P5 { get; set; }

        [DisplayName("comment")]
        public string? Comment { get; set; }
    }
}
