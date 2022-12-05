using Ao.Stock.Kata;
using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Linq.Expressions;

namespace Ao.Stock.Sample.Kata
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var compiler = new MySqlCompiler();
            var q = new MultipleQueryMetadata();
            q.Add(new SortMetadata(SortMode.Desc, new ValueMetadata<string>("email", true)));
            q.Add(new KataSelectMetadata(new Query().SelectRaw("sum(address_id)"),"sum_address_id"));
            q.Add(new GroupMetadata(new ValueMetadata<string>("store_id", true)));
            q.Add(new LimitMetadata(11));
            q.Add(new FilterMetadata
            {
                new KataMethodMetadata("like",new []
                {
                    new ValueMetadata<string>("last_name"),
                    new ValueMetadata<string>("%a%"),
                }){Compiler=compiler}
            });
            var builder = new KataQueryBuilder(compiler);
            var query = new Query().From("staff");
            builder.Merge(query, q);
            var sql = compiler.Compile(query);
            Console.WriteLine(sql);
        }
    }
}