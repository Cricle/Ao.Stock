using Ao.Stock;
using Ao.Stock.Comparering;
using Ao.Stock.Explains;
using Ao.Stock.Kata;
using Ao.Stock.SQL.Announcation;
using Ao.Stock.SQL.MySql;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

var mysql = $"server=127.0.0.1;port=3306;userid=root;password=;database=sakila;characterset=utf8mb4;";
var mt1 = StockHelper.FromType<Student1>("student");
var runner = new MySqlAutoMigrateRunner(mysql, mt1)
{
    Project = x => x.Where(y => y is not StockRenameTypeComparisonAction).ToList()
};
runner.Migrate();
var subQuery = new SqlKata.Query().From("student")
    .WhereLike("Name", "%aaa%").Limit(123);
var sql = ExplainGenerator.MySql(CompilerFetcher.Mysql.Compile(new SqlKata.Query().From(subQuery,"a").Where("Id",">","123")).ToString());
using (var dbc=runner.StockIntangible.Get<DbConnection>(new IntangibleContext { [MySqlStockIntangible.ConnectionStringKey]=mysql}))
{
	var set = ExplainResultSet<MySqlExplainResult>.FromDbConnection(dbc,sql);
    Console.WriteLine(set);
}

record class Student1([property: Key] long Id, [property: SqlIndex][property: MaxLength(20)] string Name);
