﻿using Ao.Stock.Querying;
using SqlKata;
using SqlKata.Compilers;
using System;

namespace Ao.Stock.Kata
{
    public class KataQueryBuilder
    {
        public KataQueryBuilder(Compiler compiler)
        {
            Compiler = compiler;
        }

        public Compiler Compiler { get; }

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
            else if (metadata is KataSelectMetadata kataSelect)
            {
                MergeSelectKata(query, kataSelect);
            }
            else if (metadata is KataWhereMetadata kataWhere)
            {
                MergeWhereKata(query, kataWhere);
            }
            else if (metadata is MethodMetadata method)
            {
                query.WhereRaw(MergeMethod(method));
            }
            else if (metadata is MultipleQueryMetadata muliple)
            {
                for (int i = 0; i < muliple.Count; i++)
                {
                    Merge(query, muliple[i]);
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
        public void MergeSelectKata(Query query, KataSelectMetadata metadata)
        {
            query.Select(metadata.Query, metadata.Alias);
        }
        public void MergeWhereKata(Query query, KataWhereMetadata metadata)
        {
            query.Where(_=>metadata.Query);
        }
        public string MergeMethod(MethodMetadata metadata)
        {
            if (metadata.IsMethodIgnoreCase("Contains")||
                metadata.IsMethodIgnoreCase("Like"))
            {
               return " like " + (string)MergeValue((IValueMetadata)metadata.Args[0]);
            }
            else
            {
                throw new NotSupportedException(metadata.Method);
            }
        }
        public void MergeSelect(Query query, SelectMetadata metadata)
        {
            if (metadata.Target is IValueMetadata value)
            {
                query.SelectRaw((string)MergeValue(value));
                return;
            }
            throw new NotSupportedException();
        }
        public void MergeGroup(Query query, GroupMetadata metadata)
        {
            if (metadata.Target is IValueMetadata value)
            {
                query.GroupByRaw((string)MergeValue(value));
                return;
            }
            throw new NotSupportedException();
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
                    query.OrderByRaw((string)MergeValue(value) + " desc");
                }
                return;
            }
            throw new NotSupportedException();
        }
        public void MergeFilter(Query query, FilterMetadata metadata)
        {
            var str = metadata.Combine("and");
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
