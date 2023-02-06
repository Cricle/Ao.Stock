using Ao.Stock.Comparering;
using Ao.Stock.SQL.Announcation;
using System.ComponentModel.DataAnnotations;

namespace Ao.Stock.SQL.Sqlite
{
    public class SqliteAutoMigrateRunner : DefaultAutoMigrateRunner
    {
        public SqliteAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
            : base(connectionString, newStockType, tableName, SqliteStockIntangible.Default)
        {
        }
        public SqliteAutoMigrateRunner(string connectionString, IStockType newStockType)
           : base(connectionString, newStockType, SqliteStockIntangible.Default)
        {
        }
        public static IEnumerable<IStockComparisonAction> RemoveRangeTypeChanges(IReadOnlyList<IStockComparisonAction> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                var item = input[i];
                if (item is StockPropertyTypeChangedComparisonAction typeChanged)
                {
                    var leftRaw = GetRawType(typeChanged.Left);
                    var rightRaw = GetRawType(typeChanged.Right);
                    if (string.Equals(leftRaw, rightRaw, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    yield return item;
                }
                else
                {
                    if (item is StockAttackChangedComparisonAction attackAction &&
                        attackAction.Ups != null)
                    {
                        if (attackAction.Ups.All(x => x is StockAttributeAttack attack && (attack.Attribute is MaxLengthAttribute || attack.Attribute is StringLengthAttribute)))
                        {
                            continue;
                        }   
                    }
                    yield return item;
                }
            }
        }
        private static string? GetRawType(IStockProperty property)
        {
            var rawTypeAttr = property.GetAttributeAttacks().Where(x => x.Attribute is RawDbTypeAttribute).FirstOrDefault();
            var ret = rawTypeAttr?.Attribute is RawDbTypeAttribute raw ? raw.RawType : null;
            if (ret == null)
            {
                var typeCode = Type.GetTypeCode(property.Type);
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        ret = "INTEGER";
                        break;
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        ret = "NUMERIC";
                        break;
                    case TypeCode.DateTime:
                    case TypeCode.String:
                        ret = "TEXT";
                        break;
                    default:
                        break;
                }
            }
            return ret;
        }
    }
}
