using Ao.Stock.Comparering;
using Ao.Stock.SQL.Announcation;
using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.Sqlite
{
    [SuppressMessage("", "EF1001")]
    public class SqliteAutoMigrateRunner : IAutoMigrateRunner
    {
        public SqliteAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            NewStockType = newStockType;
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string ConnectionString { get; }

        public IStockType NewStockType { get; }

        public string TableName { get; }

        public Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? Project { get; set; }

        public void Migrate()
        {
            using (var auto = new AutoMigrationHelperBuilder()
                .WithBuilderConfig(x => x.UseSqlite(ConnectionString))
                .WithMigration(x => x.AddSQLite().WithGlobalConnectionString(ConnectionString))
                .WithScaffold(new SqliteConnection(ConnectionString), x => new SqliteDesignTimeServices().ConfigureDesignTimeServices(x))
                .Build())
            {
                auto.EnsureDatabaseCreated();
                auto.Begin(NewStockType)
                    .ScaffoldCompareAndMigrate(TableName, Project);
            }
        }

        public static IEnumerable<IStockComparisonAction> RemoveRangeTypeChanges(IReadOnlyList<IStockComparisonAction> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                var item = input[i];
                if (item is StockPropertyTypeChangedComparisonAction typeChanged)
                {
                    var leftRaw = GetRawType(typeChanged.Left);
                    var rightRaw= GetRawType(typeChanged.Right);
                    if (string.Equals(leftRaw, rightRaw, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    yield return item;
                }
                else
                {
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
