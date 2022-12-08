using Ao.Stock.Kata;
using Ao.Stock.Querying;
using ExpressionTreeToString;
using SqlKata;
using SqlKata.Compilers;
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
                new GroupMetadata(new AliasMetadata(new ValueMetadata("Name"),"Name")),
                new AliasMetadata(new SelectMetadata(new MethodMetadata(KnowsMethods.DistinctCount,new ValueMetadata("Scope"))),"sum_name"),
                new SortMetadata(SortMode.Desc,new ValueMetadata("Scope")),
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
                new LimitMetadata(10)
            };
            var ss = new List<Student>();
            for (int i = 0; i < 1000; i++)
            {
                ss.Add(new Student { Id = i, Scope = Random.Shared.NextDouble() * i, Name = "aaa" + i % 5 ,Time=DateTime.Now.AddDays(i)});
            }
            IQueryable query = ss.AsQueryable();
            MetadataHelper.Merge(ref query, m);
            var expTree = query.Expression.ToString("C#");
            var d= query.ToDynamicList();
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
    public class DynamicArguments
    {
        public DynamicArguments()
            :this(new List<object?>())
        {
        }
        public DynamicArguments(IList<object?> args)
        {
            Args = args;
        }

        public IList<object?> Args { get; }

        public int Add(object? val)
        {
            Args.Add(val);
            return Args.Count-1;
        }
    }
    public static class MetadataHelper
    {
        public static string Merge(ref IQueryable queryable,FilterMetadata filter, DynamicArguments args)
        {
            var str = string.Empty;
            foreach (var item in filter)
            {
                str += "(";
                str += MergeValue(ref queryable, item, args);
                str += ")";
            }
            return str;
        }
        public static string Merge(ref IQueryable queryable, UnaryMetadata filter, DynamicArguments args)
        {
            var token = filter.GetToken();
            var left = MergeValue(ref queryable, filter.Left, args);
            if (filter.IsPreCombine())
            {
                return token + left;
            }
            return left + token;
        }
        public static string Merge(ref IQueryable queryable, BinaryMetadata filter, DynamicArguments args)
        {
            var token = filter.GetToken();
            var left = MergeValue(ref queryable, filter.Left, args);
            var right = MergeValue(ref queryable, filter.Right, args);
            return left + token + right;
        }

        public static void Merge(ref IQueryable queryable, IQueryMetadata metadata)
        {
            var args = new DynamicArguments();
            var groups = metadata.GetChildren().OfType<GroupMetadata>().ToList();
            var selects = metadata.GetChildren().OfType<SelectMetadata>().ToList();
            var alias = metadata.GetChildren().OfType<AliasMetadata>().ToList();
            var sorts = metadata.GetChildren().OfType<SortMetadata>().ToList();
            var filters = metadata.GetChildren().OfType<FilterMetadata>().ToList();
            foreach (var item in filters)
            {
                var where = MergeValue(ref queryable, item, args);
                queryable = queryable.Where(where, args.Args.ToArray());
            }
            foreach (var item in selects)
            {
                alias.Add(new AliasMetadata(item, item.Target.ToString()));
            }
            if (groups.Count!=0)
            {
                var groupos = groups.SelectMany(x => x.Target).ToList();
                var gs=new List<string>();
                foreach (var item in groupos)
                {
                    gs.Add(MergeValue(ref queryable,item, args));
                }
                var group = $"new ({string.Join(",", gs)})";
                queryable = queryable.GroupBy(group, args.Args.ToArray());
            }
            var s = new List<string>();
            foreach (var item in alias)
            {
                s.Add($"{MergeValue(ref queryable, item.Target, args)} as {item.Alias}");
            }
            var select = $"new ({string.Join(",", s)})";
            queryable = queryable.Select(select, args.Args.ToArray());
            if (sorts.Count != 0)
            {
                var first = true;
                IOrderedQueryable? orderedQueryable = null;
                foreach (var item in sorts)
                {
                    var str = MergeValue(ref queryable,item.Target, args);
                    var mode = item.SortMode == SortMode.Asc ? "" : " DESC";
                    if (first)
                    {
                        orderedQueryable = queryable.OrderBy(str + mode, args.Args.ToArray());
                    }
                    else
                    {
                        orderedQueryable = orderedQueryable!.ThenBy(str + mode, args.Args.ToArray());
                    }
                }
                queryable = orderedQueryable!;
            }
            foreach (var item in metadata.GetChildren().OfType<SkipMetadata>())
            {
                queryable = queryable.Skip(item.Value);
            }
            foreach (var item in metadata.GetChildren().OfType<LimitMetadata>())
            {
                queryable = queryable.Take(item.Value);
            }
        }

        public static string MergeValue(ref IQueryable source,IQueryMetadata query,DynamicArguments dynArgs,bool mustQuto=false)
        {
            if (query is ValueMetadata value)
            {
                if (mustQuto||value.Quto)
                {
                    return value.Value?.ToString();
                }
                return "@" + dynArgs.Add(value.Value);
            }
            else if (query is WrapperMetadata wrapper)
            {
                return MergeValue(ref source, wrapper.Left, dynArgs,true) + 
                    MergeValue(ref source, wrapper.Target, dynArgs) +
                    MergeValue(ref source, wrapper.Right, dynArgs, true);
            }
            else if (query is AliasMetadata alias)
            {
                var target = MergeValue(ref source, alias.Target, dynArgs);
                return $"{target} as {alias.Alias}";
            }
            else if (query is MethodMetadata method)
            {
                var args = new string[method.Args!=null?method.Args.Count:0];
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = MergeValue(ref source,method.Args![i], dynArgs);
                }
                if (method.Method==KnowsMethods.DistinctCount)
                {
                    return $"Select({string.Join(",", args)}).Distinct().Count()";
                }
                return $"{method.Method}({string.Join(",",args)})";
            }
            else if (query is SelectMetadata select)
            {
                return MergeValue(ref source, select.Target, dynArgs);
            }
            else if (query is FilterMetadata filter)
            {
                return Merge(ref source, filter, dynArgs);
            }
            else if (query is UnaryMetadata unary)
            {
                return Merge(ref source, unary, dynArgs);
            }
            else if (query is BinaryMetadata binary)
            {
                return Merge(ref source, binary, dynArgs);
            }
            throw new NotSupportedException(query.ToString());
        }

    }
}