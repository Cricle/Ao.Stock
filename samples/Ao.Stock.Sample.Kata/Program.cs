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
            q.Add(new SortMetadata(SortMode.Desc, new ValueMetadata<string>("a1", true)));
            q.Add(new SelectMetadata(new ValueMetadata<string>("a2", true)));
            q.Add(new GroupMetadata(new ValueMetadata<string>("a3", true)));
            q.Add(new LimitMetadata(11));
            q.Add(new FilterMetadata
            {
                new BinaryMetadata<string,string>("a3", ExpressionType.Equal,"123")
            });
            var builder = new KataQueryBuilder(compiler);
            var query = new Query().From("student");
            builder.Merge(query, q);
            var sql = compiler.Compile(query);
            Console.WriteLine(sql);
        }
    }
}