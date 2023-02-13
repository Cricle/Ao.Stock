using Ao.Stock;
using Ao.Stock.Mirror;
using Ao.Stock.Kata;
using Ao.Stock.SQL;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.MySql;
using System.ComponentModel.DataAnnotations;
using Query = SqlKata.Query;

var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
const string tableName = "student";
var mt1 = StockHelper.FromType<Student1>(tableName);
var runner = new MySqlAutoMigrateRunner(mysql, mt1);
runner.Migrate();
using (var dbc = runner.StockIntangible.GetDbContext(mysql))
using (var scope = CompilerFetcher.Mysql.CreateScope(dbc))
{
    var any = await scope.ExecuteNoQueryAsync(new Query(tableName).AsDelete());
    await scope.ExecuteNoQueryAsync(new Query(tableName).AsInsert(new string[] { "Id", "Name" },
        Enumerable.Range(0, 10).Select(x => new object[] { x, x + "aaa" })));
    var datas = await scope.ExecuteReaderAsync(new Query(tableName).Select("Id", "Name"),
        new DelegateAsyncConverter<Student1>((reader, tk) => new Student1(reader.GetInt64(0), reader.GetString(1))));
    foreach (var item in datas)
    {
        Console.WriteLine(item);
        //Console.WriteLine(string.Join(", ",item.Select(x=>$"{x.Key}={x.Value}")));
    }
}

record class Student1([property: Key] long Id, [property: SqlIndex][property: MaxLength(20)] string Name);
