using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace Ao.Stock.SQL
{
    public class DefaultScaffoldHelper : ScaffoldHelper
    {
        public DefaultScaffoldHelper(DbConnection dbConnection, Action<IServiceCollection> serviceConfigAction)
            : base(dbConnection)
        {
            ServiceConfigAction = serviceConfigAction ?? throw new ArgumentNullException(nameof(serviceConfigAction));
        }

        public Action<IServiceCollection> ServiceConfigAction { get; }

        protected override void RegistServices(IServiceCollection services)
        {
            ServiceConfigAction(services);
        }
    }
}
