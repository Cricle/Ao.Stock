using Ao.Stock;
using Ao.Stock.Comparering;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.MySql;
using System.ComponentModel.DataAnnotations;

var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
var mt1 = StockHelper.FromType<Student1>("student");
new MySqlAutoMigrateRunner(mysql, mt1, "student")
{
    Project = x => x.Where(y => y is not StockRenameTypeComparisonAction).ToList()
}.Migrate();

record class Student1([property: Key] int Id, [property: SqlIndex][property: MaxLength(256)] string Name,[property:Required]string Scope);
