using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace Ao.Stock.SQL
{
    public abstract class ScaffoldHelper : IDisposable
    {
        protected ScaffoldHelper(DbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public DbConnection DbConnection { get; }

        public DatabaseModelFactoryOptions? DatabaseModelFactoryOptions { get; set; }

        public ModelReverseEngineerOptions? ModelReverseEngineerOptions { get; set; }

        private ServiceProvider? provider;

        public bool LeaveCloseDbConnection { get; set; } = true;

        public IModel Scaffold()
        {
            if (provider == null)
            {
                provider = CreateProvider();
            }
            var databaseModelFactory = provider.GetRequiredService<IDatabaseModelFactory>();
            var scaffoldingModelFactory = provider.GetRequiredService<IScaffoldingModelFactory>();
            var model = databaseModelFactory.Create(DbConnection, DatabaseModelFactoryOptions ?? new DatabaseModelFactoryOptions());
            return scaffoldingModelFactory.Create(model, ModelReverseEngineerOptions ?? new ModelReverseEngineerOptions());
        }

        protected virtual void RegistServices(IServiceCollection services)
        {

        }

        protected ServiceProvider CreateProvider()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkDesignTimeServices();
            RegistServices(services);
            return services.BuildServiceProvider();
        }

        public void Dispose()
        {
            provider?.Dispose();
            if (LeaveCloseDbConnection)
            {
                DbConnection?.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
