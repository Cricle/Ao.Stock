using System.Collections.Generic;
using System.Linq;

namespace Ao.Stock.Querying
{
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
