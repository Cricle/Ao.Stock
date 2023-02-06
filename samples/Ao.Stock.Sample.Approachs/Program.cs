using Ao.Stock.Comparering;
using Ao.Stock.Kata;
using Ao.Stock.SQL;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.Sqlite;
using SqlKata.Execution;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Ao.Stock.Sample.Approachs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connStr = "Data source=a.db";
            var table = "student";
            var mt1 = StockHelper.FromType<Student1>(table);
            var runner = new SqliteAutoMigrateRunner(connStr, mt1)
            {
                Project = x => SqliteAutoMigrateRunner.RemoveRangeTypeChanges(x).Where(y => y is not StockRenameTypeComparisonAction).ToList()
            };
            runner.Migrate(new AutoMigrateOptions(true));

            var ctx = SQLIntangibleContextHelper.Sqlite(connStr);
            using (var queryFactory = new QueryFactory(runner.StockIntangible.Get<DbConnection>(ctx), CompilerFetcher.Sqlite))
            {
                var columns = new string[] { "Name" };
                queryFactory.Query(table)
                    .Insert(columns, new IEnumerable<object?>[]
                    {
                        mt1.Convert(columns,new object[]{123})
                    });
                foreach (var item in queryFactory.Query(table).Get().ToList())
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
    record class Student1([property: Key] long Id, [property: SqlIndex][property: MaxLength(54)] string Name);
}