using System;
using System.Collections.Generic;

namespace Ao.Stock.Querying
{
    public class DefaultMethodTranslator<TContext> : MethodTranslator<TContext>
    {
        public DefaultMethodTranslator(IMethodWrapper wrapper)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            Wrapper = wrapper;
        }
        public DefaultMethodTranslator(IDictionary<string, string> functions, IMethodWrapper wrapper)
            : base(functions, StringComparer.OrdinalIgnoreCase)
        {
            Wrapper = wrapper;
        }

        public IMethodWrapper Wrapper { get; }

        public override string? ToString(IQueryMetadata metadata,TContext context)
        {
            if (metadata is IValueMetadata valueMetadata)
            {
                if (valueMetadata.Quto)
                {
                    return Wrapper.Quto(valueMetadata.Value);
                }
                return Wrapper.WrapValue(valueMetadata.Value);
            }
            return base.ToString(metadata,context);
        }
    }

}
