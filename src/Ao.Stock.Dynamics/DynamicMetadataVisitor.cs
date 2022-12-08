using Ao.Stock.Querying;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Ao.Stock.Dynamics
{
    public class DynamicMetadataVisitor : DynamicMetadataVisitor<DefaultQueryContext>
    {
        public DynamicMetadataVisitor(IQueryable queryable) : base(queryable)
        {
        }

        public DynamicMetadataVisitor(IQueryable queryable, IList<object> args) : base(queryable, args)
        {
        }

        public override DefaultQueryContext CreateContext(IQueryMetadata metadata)
        {
            return new DefaultQueryContext();
        }
    }
    public abstract class DynamicMetadataVisitor<T> : DefaultMetadataVisitor<T>
        where T: DefaultQueryContext
    {
        private IQueryable queryable;

        public DynamicMetadataVisitor(IQueryable queryable,IList<object> args)
            :base(args)
        {
            this.queryable = queryable;
        }
        public DynamicMetadataVisitor(IQueryable queryable)
        {
            this.queryable = queryable;
        }

        public IQueryable Queryable => queryable;

        protected override void OnVisitSelect(SelectMetadata value, T context, List<string> selects)
        {
            queryable = queryable.Select($"new ({string.Join(",", selects)})", Args.ToArray());
        }
        protected override void OnVisitGroup(GroupMetadata value, T context, List<string> groups)
        {
            queryable = queryable.GroupBy($"new ({string.Join(",", groups)})", Args.ToArray());
        }
        protected override void OnVisitFilter(FilterMetadata value, IQueryMetadata metadata, T context)
        {
            queryable = queryable.Where(context.Expression, Args.ToArray());
        }

        public override void VisitLimit(LimitMetadata value, T context)
        {
            queryable = queryable.Take(value.Value);
        }
        public override void VisitSkip(SkipMetadata value, T context)
        {
            queryable = queryable.Skip(value.Value);
        }
        protected override void OnVisitMethod(MethodMetadata method, T context, string[] args)
        {
            if (method.Method == KnowsMethods.DistinctCount)
            {
                context.Expression += $"Select({string.Join(",", args)}).Distinct().Count()";
            }
            else if (method.Method == KnowsMethods.Count)
            {
                context.Expression += $"Select({string.Join(",", args)}).Count()";
            }
            else
            {
                context.Expression += $"{method.Method}({string.Join(",", args)})";
            }
        }
    }

}
