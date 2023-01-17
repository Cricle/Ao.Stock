using Ao.Stock.Comparering;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.MySql;
using System.ComponentModel.DataAnnotations;

namespace Ao.Stock.Sample.MySql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
            var mt1 = StockHelper.FromType(typeof(Student1), new ReflectionOptions { TypeNameGetter = _ => "student" });
            new MySqlAutoMigrateRunner(mysql, mt1, "student")
            {
                Project = x => x.Where(y => y is not StockRenameTypeComparisonAction).ToList()
            }.Migrate();
        }
    }
    class Student1
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [SqlIndex]
        [MaxLength(555)]
        public string Name { get; set; }
    }
}