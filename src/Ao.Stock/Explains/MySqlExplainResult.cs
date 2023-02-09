using System.ComponentModel;

namespace Ao.Stock.Explains
{
    public class MySqlExplainResult : ExplainResultBase<MySqlExplainResult>
    {
        public enum Types
        {
            System,
            Const,
            EqRef,
            Ref,
            Range,
            Index,
            All,
            Unknow
        }

        [DisplayName("id")]
        public string? Id { get; set; }

        [DisplayName("select_type")]
        public string? SelectType { get; set; }

        [DisplayName("table")]
        public string? Table { get; set; }

        [DisplayName("partitions")]
        public string? Partitions { get; set; }

        [DisplayName("type")]
        public string? Type { get; set; }

        [DisplayName("possible_keys")]
        public string? PossibleKeys { get; set; }

        [DisplayName("key")]
        public string? Key { get; set; }

        [DisplayName("key_len")]
        public string? KeyLen { get; set; }

        [DisplayName("ref")]
        public string? Ref { get; set; }

        [DisplayName("rows")]
        public string? Rows { get; set; }

        [DisplayName("filtered")]
        public string? Filtered { get; set; }

        [DisplayName("Extra")]
        public string? Extra { get; set; }

        public Types GetTypes()
        {
            switch (Type)
            {
                case "system":
                    return Types.System;
                case "const":
                    return Types.Const;
                case "eq_ref":
                    return Types.EqRef;
                case "ref":
                    return Types.Ref;
                case "range":
                    return Types.Range;
                case "index":
                    return Types.Index;
                case "ALL":
                    return Types.All;
                default:
                    return Types.Unknow;
            }
        }
    }
}
