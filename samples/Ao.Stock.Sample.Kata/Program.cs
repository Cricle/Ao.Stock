using Ao.Stock.Kata;
using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var compiler = new MySqlCompiler();
            var q = new MultipleQueryMetadata();
            q.SelectMethod(KnowsMethods.Count,"count_first_name","first_name")
                .GroupColumn("last_name");
            var query = new Query().From("staff");
            KataQueryBuilder.Mysql.Merge(query, q);
            var sql = compiler.Compile(query);
            var sqlStr = sql.ToString();
            Console.WriteLine(sqlStr);
        }
    }
}