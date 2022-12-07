using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ao.Stock.Kata
{
    public class KataQueryBuilder
    {
        public static readonly KataQueryBuilder Mysql = new KataQueryBuilder(new MySqlCompiler(), new DefaultMethodTranslator<Compiler>(KnowsMethods.Functions,DefaultMethodWrapper<Compiler>.MySql));

        public KataQueryBuilder(Compiler compiler, MethodTranslator<Compiler> translator)
        {
            Compiler = compiler;
            Translator = translator;
        }

        public Compiler Compiler { get; }

        public MethodTranslator<Compiler> Translator { get; }

        public SqlResult MergeAndCompile(Query query, IQueryMetadata metadata)
        {
            Merge(query, metadata);
            return Compiler.Compile(query);
        }

        public void Merge(Query query, IQueryMetadata metadata)
        {
            if (metadata is SelectMetadata select)
            {
                MergeSelect(query, select);
            }
            else if (metadata is GroupMetadata group)
            {
                MergeGroup(query, group);
            }
            else if (metadata is SortMetadata sort)
            {
                MergeSort(query, sort);
            }
            else if (metadata is AliasMetadata alias)
            {
                MergeAlias(query, alias);
            }
            else if (metadata is FilterMetadata filter)
            {
                MergeFilter(query, filter);
            }
            else if (metadata is LimitMetadata limit)
            {
                MergeLimit(query, limit);
            }
            else if (metadata is SkipMetadata skip)
            {
                MergeSkip(query, skip);
            }
            else if (metadata is MethodMetadata method)
            {
                query.WhereRaw(MergeMethod(method));
            }
            else if (metadata is FilterMetadata filters)
            {
                foreach (var item in filters)
                {
                    if (item is IUnaryMetadata unary)
                    {
                        query.Where(MergeUnary(unary));
                    }
                    else if (item is IBinaryMetadata binary)
                    {
                        query.Where(MergeBinary(binary));
                    }
                }
            }
            else if (metadata is IEnumerable<IQueryMetadata> muliple)
            {
                foreach (var item in muliple)
                {
                    Merge(query, item);
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public object MergeValue(IValueMetadata valueMetadata)
        {
            if (valueMetadata.Quto)
            {
                return Compiler.Wrap((string)valueMetadata.Value);
            }
            return valueMetadata.Value;
        }
        public void MergeAlias(Query query, AliasMetadata metadata)
        {
            var subQuery = new Query();
            Merge(subQuery, metadata.Target);
            query.Select(subQuery, metadata.Alias);
        }
        public string MergeMethod(MethodMetadata metadata)
        {
            return Translator.Translate(metadata, Compiler);
        }
        public void MergeSelect(Query query, SelectMetadata metadata)
        {
            if (metadata.Target is IValueMetadata value)
            {
                query.SelectRaw(MergeValue(value)?.ToString());
            }
            else if (metadata.Target is MethodMetadata method)
            {
                query.SelectRaw(MergeMethod(method));
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public void MergeGroup(Query query, GroupMetadata metadata)
        {
            if (metadata.Target is IValueMetadata value)
            {
                query.GroupByRaw((string)MergeValue(value));
            }
            else if (metadata.Target is MethodMetadata method)
            {
                query.GroupByRaw(MergeMethod(method));
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public string MergeQuery(IQueryMetadata query)
        {
            string leftStr;
            if (query is IValueMetadata valueMetadata)
            {
                leftStr = MergeValue(valueMetadata)?.ToString();
            }
            else if (query is MethodMetadata methodMetadata)
            {
                leftStr = MergeMethod(methodMetadata)?.ToString();
            }
            else
            {
                leftStr = query?.ToString();
            }
            if (leftStr==null)
            {
                leftStr = "null";
            }
            return leftStr;
        }
        public string MergeUnary(IUnaryMetadata unaryMetadata)
        {
            return new UnaryMetadata(MergeQuery(unaryMetadata.Left), unaryMetadata.ExpressionType).ToString();   
        }
        public string MergeBinary(IBinaryMetadata binaryMetadata)
        {
            var left = binaryMetadata.Left;
            var right = binaryMetadata.Right;
            if (binaryMetadata.ExpressionType== ExpressionType.Equal)
            {
                return left + " = " + right;
            }
            return new BinaryMetadata(left,binaryMetadata.ExpressionType,right).ToString();
        }
        public void MergeSort(Query query, SortMetadata metadata)
        {
            if (metadata.Target is IValueMetadata value)
            {
                if (metadata.SortMode == SortMode.Asc)
                {
                    query.OrderByRaw((string)MergeValue(value));
                }
                else if (metadata.SortMode == SortMode.Desc)
                {
                    query.OrderByRaw((string)MergeValue(value) + " DESC");
                }
            }
            if (metadata.Target is MethodMetadata method)
            {
                if (metadata.SortMode == SortMode.Asc)
                {
                    query.OrderByRaw(MergeMethod(method));
                }
                else if (metadata.SortMode == SortMode.Desc)
                {
                    query.OrderByRaw(MergeMethod(method) + " DESC");
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public void MergeFilter(Query query, FilterMetadata metadata)
        {
            var str = metadata.Combine("AND");
            query.WhereRaw(str);
        }
        public void MergeLimit(Query query, LimitMetadata metadata)
        {
            query.Limit(metadata.Value);
        }
        public void MergeSkip(Query query, SkipMetadata metadata)
        {
            query.Skip(metadata.Value);
        }
    }
}
