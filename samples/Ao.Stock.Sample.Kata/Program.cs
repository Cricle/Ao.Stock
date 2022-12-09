﻿using Ao.Stock.Kata;
using Ao.Stock.Querying;
using SqlKata;
using System.Linq.Expressions;

namespace Ao.Stock.Sample.Kata
{

    internal class Program
    {
        static void Main(string[] args)
        {

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