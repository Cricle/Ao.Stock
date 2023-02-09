using Ao.Stock;
using Ao.Stock.Comparering;
using Ao.Stock.Explains;
using Ao.Stock.Kata;
using Ao.Stock.Mirror;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.MSSQL;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

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
using (var dbc = runner.StockIntangible.Get<DbConnection>(new IntangibleContext { [SqlServerStockIntangible.ConnectionStringKey] = connStr }))
{
    await dbc.ExecuteNoQueryAsync(ExplainGenerator.SqlServerStart);
    var set = ExplainResultSet<SqlServerExplainResult>.FromDbConnection(dbc, sql);
    await dbc.ExecuteNoQueryAsync(ExplainGenerator.SqlServerEnd);
    Console.WriteLine(set);
}

record class Student1([property: Key] long Id, [property: SqlIndex][property: MaxLength(20)] string Name);
