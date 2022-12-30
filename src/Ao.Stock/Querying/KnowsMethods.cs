using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ao.Stock.Querying
{
    public static class KnowsMethods
    {
        public static readonly IReadOnlyDictionary<string, string> KnowsMethodNames;

        private static readonly string[] NotIncludes = new string[]
        {
            nameof(AllPlaceholder),
            nameof(RangeSkip1),
            nameof(AllOrAlso),
        };

        static KnowsMethods()
        {
            KnowsMethodNames = typeof(KnowsMethods).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(string) && !NotIncludes.Contains(x.Name))
                .ToDictionary(x => x.Name, x => (string)x.GetValue(null));
        }

        public static Dictionary<string, string> Functions => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [Year] = "YEAR({1})",
            [Month] = "MONTH({1})",
            [Day] = "DAY({1})",
            [Hour] = "HOUR({1})",
            [Minute] = "MINUTE({1})",
            [Second] = "SECOND({1})",
            [Microsecond] = "MICROSECOND({1})",
            [Date] = "DATE({1})",
            [Datediff] = "DATEDIFF({1})",
            [Now] = "NOW()",
            [DateFormat] = "DATE_FORMAT({1},{2})",
            [Weak] = "WEEK({1},{2})",
            [Quarter] = "QUARTER({1})",

            [Like] = "{1} like {2}",
            [StrLen] = "CHAR_LENGTH({1})",
            [StrIndexOf] = "LOCATE({1},{2})",
            [StrLowercase] = "LOWER({1})",
            [StrUppercase] = "UPPER({1}",
            [StrLeft] = "LEFT({1})",
            [StrRight] = "RIGHT({1}",
            [StrTrim] = "TRIM({1})",
            [StrConcat] = $"CONCAT({AllPlaceholder})",
            [StrCmp] = "STRCMP({1},{2})",
            [StrSub] = "SUBSTR({1},{2},{3})",
            [StrReverse] = "REVERSE({1})",

            [Cos] = "COS({1})",
            [Acos] = "ACOS({1})",
            [Abs] = "ABS({1})",

            [Count] = "COUNT({1})",
            [Sum] = "SUM({1})",
            [Avg] = "AVG({1})",
            [Min] = "MIN({1})",
            [Max] = "MAX({1})",
            [DistinctCount] = $"COUNT(DISTINCT {AllPlaceholder})",

            [Add] = "{1} + {2}",
            [Sub] = "{1} - {2}",
            [Power] = "{1} * {2}",
            [Div] = "{1} / {2}",
            [Mod] = "{1} % {2}",

            [In] = $"{{1}} in ({RangeSkip1})",
            [NotIn] = $"{{1}} not in ({RangeSkip1})"
        };

        public const string AllPlaceholder = "{**}";
        public const string AllOrAlso = "{*||*}";

        public const string RangeSkip1 = "{1-~}";


        public const string Year = "year";
        public const string Month = "month";
        public const string Day = "day";
        public const string Hour = "hour";
        public const string Minute = "minute";
        public const string Second = "second";
        public const string Microsecond = "microsecond";
        public const string Like = "like";

        public const string StrLen = "strlen";
        public const string StrLowercase = "strlowercase";
        public const string StrUppercase = "struppercase";
        public const string StrIndexOf = "strindexof";
        public const string StrLeft = "strleft";
        public const string StrRight = "strright";
        public const string StrTrim = "strtrim";
        public const string StrConcat = "strconcat";
        public const string StrSub = "strsubstring";
        public const string StrCmp = "strcmp";
        public const string StrReverse = "strreverse";

        public const string Abs = "abs";
        public const string Cos = "cos";
        public const string Acos = "acos";

        public const string Date = "date";
        public const string Datediff = "datediff";
        public const string Now = "now";
        public const string DateFormat = "DATE_FORMAT";
        public const string Weak = "weak";
        public const string Quarter = "quarter";

        public const string Sum = "sum";
        public const string Avg = "avg";
        public const string Min = "min";
        public const string Max = "max";
        public const string Count = "count";
        public const string DistinctCount = "distinctcount";

        public const string Add = "add";
        public const string Sub = "sub";
        public const string Div = "div";
        public const string Power = "power";
        public const string Mod = "mod";

        public const string In = "in";
        public const string NotIn = "notin";
    }

}
