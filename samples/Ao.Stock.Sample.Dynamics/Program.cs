using Ao.Stock.Dynamics;
using Ao.Stock.Querying;
using ExpressionTreeToString;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Ao.Stock.Sample.Dynamics
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
                new SelectMetadata(new IQueryMetadata[]
                {
                    new AliasMetadata(new ValueMetadata("it.Key.Name",true),"sum_namedd"),
                    new AliasMetadata(new MethodMetadata(KnowsMethods.DistinctCount,new ValueMetadata("Scope")),"sum_name"),
                    new AliasMetadata(new MethodMetadata(KnowsMethods.Count,new ValueMetadata("Scope")),"count_name"),
                }),
                new SortMetadata(SortMode.Desc,new ValueMetadata("count_name",true)),
                new SortMetadata(SortMode.Asc,new ValueMetadata("sum_name",true)),
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
            Console.WriteLine(expTree);
            var d = query.ToDynamicList();
        }
    }
}