using Ao.Stock.Kata;
using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;
using System.Linq.Expressions;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var q = new MultipleQueryMetadata();
            q.Add(
                new MethodMetadata(KnowsMethods.In,
                    new ValueMetadata("staff_id", true),
                    new ValueMetadata(1),
                    new ValueMetadata(2),
                    new ValueMetadata(3)));
            q.Add(new FilterMetadata
            {
                new BinaryMetadata("last_update", ExpressionType.GreaterThanOrEqual, "2022-12-7 12:38:00")
            });
            var sql = KataQueryBuilder.Mysql.MergeAndCompile(new Query().From("staff"), q);
            var sqlStr = sql.ToString();
            Console.WriteLine(sqlStr);
        }
    }
}