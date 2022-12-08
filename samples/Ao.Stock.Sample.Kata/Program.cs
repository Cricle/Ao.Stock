using Ao.Stock.Dynamics;
using Ao.Stock.Kata;
using Ao.Stock.Querying;
using ExpressionTreeToString;
using SqlKata;
using SqlKata.Compilers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace Ao.Stock.Sample.Kata
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Scope { get; set; }

        public DateTime Time { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var m = new MultipleQueryMetadata
            {
                new FilterMetadata
                {
                    new BinaryMetadata(WrapperMetadata.Brackets(new BinaryMetadata(
                            new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.GreaterThan,new ValueMetadata(DateTime.Parse("2022-12-8"))),
                            ExpressionType.OrElse,
                            new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.LessThan,new ValueMetadata(DateTime.Parse("2023-12-8"))))),
                         ExpressionType.AndAlso,
                        new BinaryMetadata(
                            new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.GreaterThan,new ValueMetadata(DateTime.Parse("2022-12-8"))),
                            ExpressionType.OrElse,
                            new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.LessThan,new ValueMetadata(DateTime.Parse("2023-12-8")) )
                    ))
                },
                new GroupMetadata(new AliasMetadata(new ValueMetadata("Name"),"Name")),
                new SelectMetadata(new AliasMetadata(new MethodMetadata(KnowsMethods.DistinctCount,new ValueMetadata("Scope")),"sum_name")),
                new SortMetadata(SortMode.Desc,new ValueMetadata("Scope")),
                new LimitMetadata(10)
            };
            var ss = new List<Student>();
            for (int i = 0; i < 1000; i++)
            {
                ss.Add(new Student { Id = i, Scope = Random.Shared.NextDouble() * i, Name = "aaa" + i % 5, Time = DateTime.Now.AddDays(i) });
            }
            IQueryable query = ss.AsQueryable();
            var visitor = new DynamicMetadataVisitor(query);
            visitor.Visit(m, visitor.CreateContext(m));
            query = visitor.Queryable;
            var expTree = query.Expression.ToString("C#");
            var d = query.ToDynamicList();
            return;
            var msub = new MultipleQueryMetadata
            {
                new FilterMetadata
                {
                    new BinaryMetadata(
                        new BinaryMetadata("last_update", ExpressionType.GreaterThanOrEqual, "2022-12-7 12:38:00"),
                        ExpressionType.OrElse,
                        new BinaryMetadata("last_update", ExpressionType.GreaterThanOrEqual, "2022-12-8 12:38:00"))
                }
            };
            Console.WriteLine(msub);
            var q = new MultipleQueryMetadata();
            var builder = KataQueryBuilder.Mysql;
            var root = new Query().From("staff");
            builder.Merge(root, msub);
            var sqlsub = builder.Compiler.Compile(root);
            var sql = builder.MergeAndCompile(new Query().FromRaw($"({sqlsub})"), q);
            var sqlStr = sql.ToString();
            Console.WriteLine(sqlStr);
        }
    }

}