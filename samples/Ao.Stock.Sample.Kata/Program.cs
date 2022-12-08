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
            //var m = new MultipleQueryMetadata
            //{
            //    new FilterMetadata
            //    {
            //        new BinaryMetadata(WrapperMetadata.Brackets(new BinaryMetadata(
            //                new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.GreaterThan,new ValueMetadata(DateTime.Parse("2022-12-8"))),
            //                ExpressionType.OrElse,
            //                new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.LessThan,new ValueMetadata(DateTime.Parse("2023-12-8"))))),
            //             ExpressionType.AndAlso,
            //            new BinaryMetadata(
            //                new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.GreaterThan,new ValueMetadata(DateTime.Parse("2022-12-8"))),
            //                ExpressionType.OrElse,
            //                new BinaryMetadata(new ValueMetadata("Time",true), ExpressionType.LessThan,new ValueMetadata(DateTime.Parse("2023-12-8")) )
            //        ))
            //    },
            //    new GroupMetadata(new AliasMetadata(new ValueMetadata("Name"),"Name")),
            //    new SelectMetadata(new IQueryMetadata[]
            //    {
            //        new AliasMetadata(new MethodMetadata(KnowsMethods.DistinctCount,new ValueMetadata("Scope")),"sum_name"),
            //        new AliasMetadata(new MethodMetadata(KnowsMethods.Count,new ValueMetadata("Scope")),"count_name"),
            //    }),
            //    new SortMetadata(SortMode.Desc,new ValueMetadata("Scope")),
            //    new LimitMetadata(10)
            //};
            //var ss = new List<Student>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    ss.Add(new Student { Id = i, Scope = Random.Shared.NextDouble() * i, Name = "aaa" + i % 5, Time = DateTime.Now.AddDays(i) });
            //}
            //IQueryable query = ss.AsQueryable();
            //var visitor = new DynamicMetadataVisitor(query);
            //visitor.Visit(m, visitor.CreateContext(m));
            //query = visitor.Queryable;
            //var expTree = query.Expression.ToString("C#");
            //var d = query.ToDynamicList();
            //return;

            var msub = new MultipleQueryMetadata
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
                new SelectMetadata(new IQueryMetadata[]
                {
                    new AliasMetadata(new MethodMetadata(KnowsMethods.DistinctCount,new ValueMetadata("Scope")),"sum_name"),
                    new AliasMetadata(new MethodMetadata(KnowsMethods.Count,new ValueMetadata("Scope")),"count_name"),
                }),
                new SortMetadata(SortMode.Desc,new ValueMetadata("Scope")),
                new LimitMetadata(10)
            };
            Console.WriteLine(msub);
            var root = new Query().From("staff");
            var builder = KataMetadataVisitor.Mysql(root);
            builder.Visit(msub, builder.CreateContext(msub));
            var sqlsub = builder.Compiler.Compile(root);
            var sqlStr = sqlsub.ToString();
            Console.WriteLine(sqlStr);
        }
    }

}