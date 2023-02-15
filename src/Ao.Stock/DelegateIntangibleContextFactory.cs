using System;

namespace Ao.Stock
{
    public class DelegateIntangibleContextFactory : IIntangibleContextFactory
    {
        public DelegateIntangibleContextFactory(Func<IIntangibleContext> creator)
        {
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
        }

        public Func<IIntangibleContext> Creator { get; }

        public IIntangibleContext Create()
        {
            return Creator();
        }
    }
}
