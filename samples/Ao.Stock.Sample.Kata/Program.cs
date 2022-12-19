using Ao.Stock.Kata;
using Ao.Stock.Querying;
using DynamicExpresso;
using SqlKata;
using System.Linq.Expressions;

namespace Ao.Stock.Sample.Kata
{
    public class DynamicBuilder
    {
        public DynamicBuilder()
            :this(new Interpreter())
        {
        }

        public DynamicBuilder(Interpreter interpreter)
        {
            Interpreter = interpreter;
        }

        public Interpreter Interpreter { get; }

        public IQueryMetadata As(string exp, params Parameter[] parameters)
        {
            return ExpressionParser.Default.Parse(Interpreter.Parse(exp, parameters).Expression);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var pars = new Parameter[]
            {
                new Parameter("last_update",typeof(DateTime?)),
                new Parameter("first_name",typeof(string)),
                new Parameter("store_id",typeof(int?)),
            };
            var b = new DynamicBuilder();
            b.Interpreter
                .SetVariable("dt1", DateTime.Parse("2022-12-8"));
            var msub = new MultipleQueryMetadata
            {
                new FilterMetadata
                {
                    b.As("(last_update > dt1||(last_update<dt1)&&store_id==123)",pars)
                },
                new GroupMetadata(new ValueMetadata("first_name",true)),
                new SelectMetadata(new IQueryMetadata[]
                {
                    new AliasMetadata(new MethodMetadata(KnowsMethods.DistinctCount,new ValueMetadata("store_id",true)),"sum_name"),
                    new AliasMetadata(new MethodMetadata(KnowsMethods.Count,new ValueMetadata("store_id",true)),"count_name"),
                    new AliasMetadata(new ValueMetadata("first_name", true),"rawname"),
                }),
                new SortMetadata(SortMode.Desc,new MethodMetadata(KnowsMethods.Count,new ValueMetadata("store_id",true))),
                new SortMetadata(SortMode.Asc,new MethodMetadata(KnowsMethods.Avg,new ValueMetadata("store_id",true))),
                new LimitMetadata(10)
            };
            Console.WriteLine(msub);
            Console.WriteLine();
            var root = new Query().From("staff");
            var builder = KataMetadataVisitor.Mysql(root);
            builder.Visit(msub, builder.CreateContext(msub));
            var sqlsub = builder.Compiler.Compile(root);
            var sqlStr = sqlsub.ToString();
            Console.WriteLine(sqlStr);
        }
    }

}