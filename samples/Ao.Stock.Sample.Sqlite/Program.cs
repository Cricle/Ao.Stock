using Ao.Stock.Comparering;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.Sqlite;
using System.ComponentModel.DataAnnotations;

namespace Ao.Stock.Sample.Sqlite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mysql = $"Data source=a.db;";
            var mt1 = StockHelper.FromType<Student1>("student");
            new SqliteAutoMigrateRunner(mysql, mt1, "student")
            {
                Project = x => SqliteAutoMigrateRunner.RemoveRangeTypeChanges(x).Where(y => y is not StockRenameTypeComparisonAction).ToList()
            }.Migrate();
        }
    }
    record class Student1
    {
        [Key]
        public long Id { get; set; }

        [SqlIndex]
        [MaxLength(54)]
        public string Name { get; set; }

        //[SqlIndex]
        //public double Scope { get; set; }
    }
}