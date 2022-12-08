using Ao.Stock.Querying;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Ao.Stock.Dynamics
{
    public class DynamicMetadataVisitor : DynamicMetadataVisitor<DynamicQueryContext>
    {
        public DynamicMetadataVisitor(IQueryable queryable) : base(queryable)
        {
        }

        public DynamicMetadataVisitor(IQueryable queryable, IList<object> args) : base(queryable, args)
        {
        }

        public override DynamicQueryContext CreateContext(IQueryMetadata metadata)
        {
            return new DynamicQueryContext();
        }
    }
    public abstract class DynamicMetadataVisitor<T> : MetadataVisitor<T>
        where T: DynamicQueryContext
    {
        private IQueryable queryable;

        public DynamicMetadataVisitor(IQueryable queryable,IList<object> args)
        {
            this.queryable = queryable;
            Args = args;
        }
        public DynamicMetadataVisitor(IQueryable queryable)
            :this(queryable,new List<object>(0))
        {
        }

        public IQueryable Queryable => queryable;

        public IList<object> Args { get; }

        public int Add(object val)
        {
            Args.Add(val);
            return Args.Count - 1;
        }

        public abstract T CreateContext(IQueryMetadata metadata);

        public override void VisitSelect(SelectMetadata value, T context)
        {
            var selects = new List<string>();
            for (int i = 0; i < value.Target.Count; i++)
            {
                var target = value.Target[i];
                var ctx = CreateContext(target);
                Visit(target, ctx);
                selects.Add(ctx.Expression);
            }
            queryable = queryable.Select($"new ({string.Join(",", selects)})", Args.ToArray());
        }

        public override void VisitGroup(GroupMetadata value, T context)
        {
            var groups = new List<string>();
            for (int i = 0; i < value.Target.Count; i++)
            {
                var item = value.Target[i];
                var ctx = CreateContext(item);
                Visit(item, ctx);
                groups.Add(ctx.Expression);
            }
            queryable = queryable.GroupBy($"new ({string.Join(",", groups)})", Args.ToArray());
        }

        public override void VisitFilter(FilterMetadata value, T context)
        {
            for (int i = 0; i < value.Count; i++)
            {
                var item = value[i];
                var ctx = CreateContext(item);
                Visit(item, ctx);
                queryable = queryable.Where(ctx.Expression, Args.ToArray());
            }
        }

        public override void VisitBinary(BinaryMetadata value, T context)
        {
            var token = value.GetToken();
            var leftCtx = CreateContext(value.Left);
            var rightCtx = CreateContext(value.Right);
            Visit(value.Left, leftCtx);
            Visit(value.Right, rightCtx);
            context.Expression += leftCtx.Expression + token + rightCtx.Expression;
        }

        public override void VisitUnary(UnaryMetadata value, T context)
        {
            var token = value.GetToken();
            var ctx = CreateContext(value.Left);
            Visit(value.Left, ctx);
            if (value.IsPreCombine())
            {
                context.Expression += token + ctx.Expression;
            }
            else
            {
                context.Expression += ctx.Expression + token;
            }
        }
        public override void VisitLimit(LimitMetadata value, T context)
        {
            queryable = queryable.Take(value.Value);
        }
        public override void VisitSkip(SkipMetadata value, T context)
        {
            queryable = queryable.Skip(value.Value);
        }
        public override void VisitWrapper(WrapperMetadata value, T context)
        {
            var leftCtx = CreateContext(value.Left);
            leftCtx.MustQuto = true;
            var targetCtx = CreateContext(value.Target);
            var rightCtx = CreateContext(value.Right);
            rightCtx.MustQuto = true;
            Visit(value.Left, leftCtx);
            Visit(value.Target, targetCtx);
            Visit(value.Right, rightCtx);
            context.Expression += leftCtx.Expression + targetCtx.Expression + rightCtx.Expression;
        }
        public override void VisitAlias(AliasMetadata value, T context)
        {
            var ctx = CreateContext(value.Target);
            Visit(value.Target, ctx);
            context.Expression += $"{ctx.Expression} as {value.Alias}";
        }

        public override void VisitValue(ValueMetadata value, T context)
        {
            if (context.MustQuto || value.Quto)
            {
                context.Expression += value.Value?.ToString() ?? "null";
            }
            else
            {
                context.Expression += "@" + Add(value.Value);
            }
        }
        public override void VisitMethod(MethodMetadata method, T context)
        {
            var args = new string[method.Args != null ? method.Args.Count : 0];
            for (int i = 0; i < args.Length; i++)
            {
                var arg = method.Args[i];
                var ctx = CreateContext(arg);
                Visit(arg, ctx);
                args[i] = ctx.Expression;
            }
            if (method.Method == KnowsMethods.DistinctCount)
            {
                context.Expression += $"Select({string.Join(",", args)}).Distinct().Count()";
            }
            else
            {
                context.Expression += $"{method.Method}({string.Join(",", args)})";
            }
        }
    }

}
