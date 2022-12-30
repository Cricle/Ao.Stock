using Microsoft.Extensions.DependencyInjection;

namespace Ao.Stock.SQL
{
    public class DesignTimeServiceBox
    {
        public DesignTimeServiceBox(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}
