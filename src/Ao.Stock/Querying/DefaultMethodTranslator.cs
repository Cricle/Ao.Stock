using System;
using System.Collections.Generic;

namespace Ao.Stock.Querying
{
    public class DefaultMethodTranslator<TContext> : MethodTranslator<TContext>
    {
        public DefaultMethodTranslator(IMethodWrapper<TContext> wrapper)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            Wrapper = wrapper;
        }
        public DefaultMethodTranslator(IDictionary<string, string> functions, IMethodWrapper<TContext> wrapper)
            : base(functions, StringComparer.OrdinalIgnoreCase)
        {
            Wrapper = wrapper;
        }

        public IMethodWrapper<TContext> Wrapper { get; }

        public override string? ToString(IQueryMetadata metadata, TContext context)
        {
            if (metadata is IValueMetadata valueMetadata)
            {
                if (valueMetadata.Quto)
                {
                    return Wrapper.Quto(valueMetadata.Value, context);
                }
                return Wrapper.WrapValue(valueMetadata.Value, context);
            }
            return base.ToString(metadata, context);
        }
    }

}
