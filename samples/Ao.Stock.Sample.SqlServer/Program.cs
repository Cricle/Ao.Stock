using Ao.Stock;
using Ao.Stock.Comparering;
using Ao.Stock.Explains;
using Ao.Stock.Kata;
using Ao.Stock.Mirror;
using Ao.Stock.SQL;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.MSSQL;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;

var connStr = $"Data Source=127.0.0.1,1433;Initial Catalog=student;User Id=sa;Password=355343; ";
var mt1 = StockHelper.FromType<Student1>("student");
var runner = new SqlServerAutoMigrateRunner(connStr, mt1)
{
    Project = x => x.Where(y => y is not StockRenameTypeComparisonAction).ToList()
};
runner.Migrate();
var subQuery = new SqlKata.Query().From("student")
    .WhereLike("Name", "%aaa%").Limit(123);
var sql = CompilerFetcher.SqlServer.Compile(new SqlKata.Query().From(subQuery, "a").Where("Id", ">", "123")).ToString();
var stock = SqlServerStockIntangible.Default;
var pool = stock.CreateDbConnectionPool(stock.CreateContext(connStr), 2);
var tsks = Enumerable.Range(0, 10).Select(x =>
{
    var t = x;
    return Task.Factory.StartNew(async () =>
     {
         while (true)
         {
             var sw = Stopwatch.GetTimestamp();
             var dbc = pool.Get();
             //using var dbc = SqlServerStockIntangible.Default.Get<DbConnection>(new IntangibleContext { [SqlServerStockIntangible.ConnectionStringKey] = connStr });
             try
             {
                 if (dbc.State != ConnectionState.Open)
                 {
                     dbc.Open();
                 }
                 await dbc.ExecuteNoQueryAsync(ExplainGenerator.SqlServerStart);
                 var set = ExplainResultSet<SqlServerExplainResult>.FromDbConnection(dbc, sql);
                 await dbc.ExecuteNoQueryAsync(ExplainGenerator.SqlServerEnd);
             }
             finally
             {
                 pool.Return(dbc);
                 Console.WriteLine($"[{t}]:" + new TimeSpan(Stopwatch.GetTimestamp() - sw));
             }
             await Task.Delay(1000);
         }
     }).Unwrap();
});
await Task.WhenAll(tsks);

record class Student1([property: Key] long Id, [property: SqlIndex][property: MaxLength(20)] string Name);
