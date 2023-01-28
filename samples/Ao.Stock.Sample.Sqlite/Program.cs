using Ao.Stock;
using Ao.Stock.Comparering;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.Sqlite;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

var mt1 = StockHelper.FromType<Student1>("student");
var sw = Stopwatch.GetTimestamp();
var runner = new SqliteAutoMigrateRunner("Data source=a.db;", mt1, "student")
{
    Project = x => SqliteAutoMigrateRunner.RemoveRangeTypeChanges(x).Where(y => y is not StockRenameTypeComparisonAction).ToList()
};
runner.Migrate();
Console.WriteLine(new TimeSpan(Stopwatch.GetTimestamp() - sw));

record class Student1([property: Key] long Id, [property: SqlIndex][property: MaxLength(54)] string Name);
