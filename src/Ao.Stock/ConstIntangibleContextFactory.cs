using System;

namespace Ao.Stock
{
    public class ConstIntangibleContextFactory : IIntangibleContextFactory
    {
        public ConstIntangibleContextFactory(IIntangibleContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IIntangibleContext Context { get; }

        public IIntangibleContext Create()
        {
            return Context;
        }
    }
}
