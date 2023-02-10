using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
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
            var model = GetDatabaseModel();
            var scaffoldingModelFactory = provider!.GetRequiredService<IScaffoldingModelFactory>();
            return scaffoldingModelFactory.Create(model, ModelReverseEngineerOptions ?? new ModelReverseEngineerOptions());
        }
        public DatabaseModel? GetDatabaseModel()
        {
            if (provider == null)
            {
                provider = CreateProvider();
            }
            return GetDatabaseModel(provider);
        }
        public DatabaseModel? GetDatabaseModel(IServiceProvider provider)
        {
            var databaseModelFactory = provider.GetRequiredService<IDatabaseModelFactory>();
            var model = databaseModelFactory.Create(DbConnection, DatabaseModelFactoryOptions ?? new DatabaseModelFactoryOptions());
            return model;
        }
        public ScaffoldedModel? GetScaffoldedModel(string connectionString, ModelCodeGenerationOptions? codeGenerationOptions)
        {
            if (provider == null)
            {
                provider = CreateProvider();
            }
            return GetScaffoldedModel(connectionString, codeGenerationOptions);
        }
        public ScaffoldedModel? GetScaffoldedModel(string connectionString, ModelCodeGenerationOptions? codeGenerationOptions, IServiceProvider provider)
        {
            var databaseModelFactory = provider.GetRequiredService<IReverseEngineerScaffolder>();
            var model = databaseModelFactory.ScaffoldModel(connectionString,
                DatabaseModelFactoryOptions ?? new DatabaseModelFactoryOptions(),
                ModelReverseEngineerOptions ?? new ModelReverseEngineerOptions(),
                codeGenerationOptions ?? new ModelCodeGenerationOptions());
            return model;
        }
        protected virtual void RegistServices(IServiceCollection services)
        {

        }
        public void ConfigServices(IServiceCollection services)
        {
            services.AddEntityFrameworkDesignTimeServices();
            RegistServices(services);
        }
        public ServiceProvider CreateProvider()
        {
            var services = new ServiceCollection();
            ConfigServices(services);
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
