using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock.Querying
{
    public class DefaultQueryContext
    {
        public string Expression { get; set; }

        public bool MustQuto { get; set; }
    }
    public abstract class DefaultMetadataVisitor<T> : MetadataVisitor<T>
        where T : DefaultQueryContext
    {
        public DefaultMetadataVisitor(IList<object?> args)
        {
            Args = args;
        }
        public DefaultMetadataVisitor()
            : this(new List<object?>(0))
        {
        }

        public IList<object?> Args { get; }

        public int Add(object? val)
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
            OnVisitSelect(value, context,selects);
        }
        protected abstract void OnVisitSelect(SelectMetadata value, T context,List<string> selects);

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
            OnVisitGroup(value, context, groups);
        }
        protected abstract void OnVisitGroup(GroupMetadata value, T context, List<string> groups);

        public override void VisitFilter(FilterMetadata value, T context)
        {
            for (int i = 0; i < value.Count; i++)
            {
                var item = value[i];
                var ctx = CreateContext(item);
                Visit(item, ctx);
                OnVisitFilter(value, item, ctx);
            }
        }
        protected abstract void OnVisitFilter(FilterMetadata value,IQueryMetadata metadata, T context);

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
                var arg = method.Args![i];
                var ctx = CreateContext(arg);
                Visit(arg, ctx);
                args[i] = ctx.Expression;
            }
            OnVisitMethod(method, context, args);
        }
        protected abstract void OnVisitMethod(MethodMetadata method,T context, string[] args);
    }

    public class MetadataVisitor<TContext>
    {
        public virtual void Visit(IQueryMetadata query, TContext context)
        {
            if (query is ValueMetadata value)
            {
                VisitValue(value, context);
            }
            else if (query is WrapperMetadata wrapper)
            {
                VisitWrapper(wrapper, context);
            }
            else if (query is AliasMetadata alias)
            {
                VisitAlias(alias, context);
            }
            else if (query is MethodMetadata method)
            {
                VisitMethod(method, context);
            }
            else if (query is SelectMetadata select)
            {
                VisitSelect(select, context);
            }
            else if (query is FilterMetadata filter)
            {
                VisitFilter(filter, context);
            }
            else if (query is UnaryMetadata unary)
            {
                VisitUnary(unary, context);
            }
            else if (query is BinaryMetadata binary)
            {
                VisitBinary(binary, context);
            }
            else if (query is GroupMetadata group)
            {
                VisitGroup(group, context);
            }
            else if (query is LimitMetadata limit)
            {
                VisitLimit(limit, context);
            }
            else if (query is SkipMetadata skip)
            {
                VisitSkip(skip, context);
            }
            else if (query is IEnumerable<IQueryMetadata> querys)
            {
                foreach (var item in querys)
                {
                    Visit(item, context);
                }
            }
        }
        public virtual void VisitLimit(LimitMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitSkip(SkipMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitGroup(GroupMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitFilter(FilterMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitSelect(SelectMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitUnary(UnaryMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitBinary(BinaryMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }

        public virtual void VisitValue(ValueMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitWrapper(WrapperMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitAlias(AliasMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
        public virtual void VisitMethod(MethodMetadata value, TContext context)
        {
            foreach (var item in value.GetChildren())
            {
                Visit(item, context);
            }
        }
    }
}
