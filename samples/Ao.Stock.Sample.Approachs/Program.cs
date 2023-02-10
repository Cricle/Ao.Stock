using Ao.Stock.Comparering;
using Ao.Stock.SQL;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.Sqlite;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Z.BulkOperations;

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
            using (var conn = runner.StockIntangible.Get<DbConnection>(ctx))
            {
                conn.Open();
                using (var op = new BulkOperation(conn))
                {
                    op.Log = x => Console.WriteLine(x);
                    op.DestinationTableName = table;
                    op.ColumnInputNames = new List<string> { "Name", "qwer" };
                    op.ColumnOutputNames = new List<string> { "qwer" };
                    op.ColumnPrimaryKeyNames = new List<string> { "Name" };
                    op.BatchSize = 5;
                    op.AutoMapOutputDirection = false;
                    var ds = new Dictionary<string, object>[5];
                    for (int i = 0; i < ds.Length; i++)
                    {
                        ds[i] = new Dictionary<string, object>
                        {
                            ["Name"] = "aaa" + i,
                            ["qwer"] = "fff" + i
                        };
                    }
                    op.BulkMerge(ds);
                    //op.BulkMerge(new Student1[]
                    //{
                    //    new Student1(0,"aaa","fff"),
                    //    new Student1(0,"bvbb","qqq"),
                    //});
                }
            }

        }
    }
    record class Student1([property: Key] long Id, [property: SqlIndex][property: MaxLength(54)] string Name, string qwer);
}