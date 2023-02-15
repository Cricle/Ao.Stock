using Ao.Stock;
using Ao.Stock.Kata;
using Ao.Stock.SQL;
using Ao.Stock.SQL.MySql;
using System.ComponentModel;
using SqlKata;
using Ao.Stock.SQLKata;

var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
const string tableName = "student";
var mt1 = StockHelper.FromType<Student1>(tableName);
var runner = new MySqlAutoMigrateRunner(mysql, mt1);
var d = new StockRuntime(CompilerFetcher.Mysql,
    new ConstIntangibleContextFactory(runner.StockIntangible.CreateContext(mysql)), 
    runner.StockIntangible);
//runner.Migrate();
using (var ctx = d.CreateContext<Student1>(tableName))
{
    //await ctx.DeleteAsync();
    //await ctx.InsertAsync(Enumerable.Range(0, 10000).Select(x => new Student1
    //{
    //    Id = x,
    //    Name = "aaa" + x
    //}));
    var datas = await ctx.GetAsync();
    foreach (var item in datas)
    {
        Console.WriteLine(item);
    }
}

record class Student1
{
    public Student1() { }

    public int Id { get; set; }

    public string? Name { get; set; }
}