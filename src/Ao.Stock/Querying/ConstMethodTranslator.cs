using System.Collections.Generic;
using System;

namespace Ao.Stock.Querying
{
    public static class ConstMethodTranslator
    {
        public static Dictionary<string, string> Functions => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [KnowsMethods.Year] = "YEAR({1})",
            [KnowsMethods.Month] = "MONTH({1})",
            [KnowsMethods.Day] = "DAY({1})",
            [KnowsMethods.Hour] = "HOUR({1})",
            [KnowsMethods.Minute] = "MINUTE({1})",
            [KnowsMethods.Second] = "SECOND({1})",
            [KnowsMethods.Microsecond] = "MICROSECOND({1})",
            [KnowsMethods.Date] = "DATE({1})",
            [KnowsMethods.Datediff] = "DATEDIFF({1})",
            [KnowsMethods.Now] = "NOW()",

            [KnowsMethods.Like] = "{1} like {2}",
            [KnowsMethods.StrLen] = "CHAR_LENGTH({1})",
            [KnowsMethods.StrIndexOf] = "LOCATE({1},{2})",
            [KnowsMethods.StrLowercase] = "LOWER({1})",
            [KnowsMethods.StrUppercase] = "UPPER({1}",
            [KnowsMethods.StrLeft] = "LEFT({1})",
            [KnowsMethods.StrRight] = "RIGHT({1}",
            [KnowsMethods.StrTrim] = "TRIM({1})",
            [KnowsMethods.StrConcat] = $"CONCAT({KnowsMethods.AllPlaceholder})",
            [KnowsMethods.StrCmp] = "STRCMP({1},{2})",
            [KnowsMethods.StrSub] = "SUBSTR({1},{2},{3})",

            [KnowsMethods.Cos] = "COS({1})",
            [KnowsMethods.Acos] = "ACOS({1})",
            [KnowsMethods.Abs] = "ABS({1})",

            [KnowsMethods.Count] = "COUNT({1})",
            [KnowsMethods.Sum] = "SUM({1})",
            [KnowsMethods.Avg] = "AVG({1})",
            [KnowsMethods.Min] = "MIN({1})",
            [KnowsMethods.Max] = "MAX({1})",
            [KnowsMethods.DistinctCount] = $"COUNT(DISTINCT {KnowsMethods.AllPlaceholder})",

            [KnowsMethods.Add] = "{1} + {2}",
            [KnowsMethods.Sub] = "{1} - {2}",
            [KnowsMethods.Power] = "{1} * {2}",
            [KnowsMethods.Div] = "{1} / {2}",
            [KnowsMethods.Mod] = "{1} % {2}",
        };
    }

}
