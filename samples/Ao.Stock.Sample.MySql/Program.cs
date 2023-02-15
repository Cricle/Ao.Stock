using Ao.Stock;
using Ao.Stock.Mirror;
using Ao.Stock.Kata;
using Ao.Stock.SQL;
using Ao.Stock.SQL.MySql;
using System.Diagnostics;
using System.ComponentModel;
using SqlKata;

var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
const string tableName = "student";
var mt1 = StockHelper.FromType<Student1>(tableName);
var runner = new MySqlAutoMigrateRunner(mysql, mt1);
//runner.Migrate();
using (var dbc = runner.StockIntangible.GetDbContext(mysql))
using (var scope = CompilerFetcher.Mysql.CreateScope(dbc))
{
    var any = await scope.ExecuteNoQueryAsync(new Query(tableName).AsDelete());
    await scope.ExecuteNoQueryAsync(new Query(tableName).AsInsert(new string[] { "Id", "Name" },
        Enumerable.Range(0, 10).Select(x => new object[] { x, x + "aaa" })));
    await scope.ExecuteReaderAsync(new Query(tableName).Select("Id", "Name"),
        ORMAsyncConverter<Student1>.Default);
    var sw = Stopwatch.GetTimestamp();
    var datas = await scope.ExecuteReaderAsync(new Query(tableName).Select("Id", "Name"),
     ORMAsyncConverter<Student1>.Default);
    Console.WriteLine(new TimeSpan(Stopwatch.GetTimestamp() - sw));
    //foreach (var item in datas)
    //{
    //    Console.WriteLine(item);
    //}
}

record class Student1
{
    public Student1() { }

    [DisplayName("Id")]
    public int aId { get; set; }

    public string? Name { get; set; }
}