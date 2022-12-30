using Ao.Stock.Comparering;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace Ao.Stock.SQL
{
    public class AutoMigrationHelperBuilder
    {
        private ScaffoldHelper? scaffold;
        private MigrationHelper? migration;
        private Action<DbContextOptionsBuilder>? builderConfig;

        public AutoMigrationHelperBuilder WithScaffold(ScaffoldHelper helper)
        {
            scaffold = helper;
            return this;
        }
        public AutoMigrationHelperBuilder WithScaffold(DbConnection connection, Action<IServiceCollection> serviceConfigAction)
        {
            return WithScaffold(new DefaultScaffoldHelper(connection, serviceConfigAction));
        }
        public AutoMigrationHelperBuilder WithMigration(MigrationHelper helper)
        {
            migration = helper;
            return this;
        }
        public AutoMigrationHelperBuilder WithMigration(Action<IMigrationRunnerBuilder> runnerBuilderAction)
        {
            return WithMigration(new DefaultMigrationHelper(runnerBuilderAction));
        }
        public AutoMigrationHelperBuilder WithBuilderConfig(Action<DbContextOptionsBuilder> builderConfig)
        {
            this.builderConfig = builderConfig;
            return this;
        }
        public AutoMigrationHelper Build()
        {
            if (scaffold == null || migration == null || builderConfig == null)
            {
                throw new InvalidOperationException("scaffold, migration, builderConfig must not null");
            }
            return new AutoMigrationHelper(scaffold, migration, builderConfig);
        }
    }
    public class AutoMigrationHelper : IDisposable
    {
        public ScaffoldHelper ScaffoldHelper { get; }

        public MigrationHelper MigrationHelper { get; }

        public Action<DbContextOptionsBuilder> BuilderConfig { get; }

        private DbContext? dbContext;

        public AutoMigrationHelper(ScaffoldHelper scaffoldHelper, MigrationHelper migrationHelper, Action<DbContextOptionsBuilder> builderConfig)
        {
            ScaffoldHelper = scaffoldHelper;
            MigrationHelper = migrationHelper;
            BuilderConfig = builderConfig;
        }

        public void Dispose()
        {
            dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool EnsureDatabaseCreated()
        {
            if (dbContext == null)
            {
                var opt = new DbContextOptionsBuilder();
                BuilderConfig(opt);
                var opts = opt.Options;
                opts.GetExtension<CoreOptionsExtension>().WithServiceProviderCachingEnabled(false);
                dbContext = new DbContext(opts);
            }
            return dbContext.Database.EnsureCreated();
        }
        public BeginScaffoldMigration Begin(IStockType newType)
        {
            return Begin(newType, DefaultStockTypeComparer.Default);
        }
        public BeginScaffoldMigration Begin(IStockType newType, IStockTypeComparer stockTypeComparer)
        {
            return new BeginScaffoldMigration(this, newType, stockTypeComparer);
        }
    }
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
