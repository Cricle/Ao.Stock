using Ao.Stock.Comparering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Ao.Stock.SQL
{
    public class DesignTimeProvider : IDisposable, IServiceProvider
    {
        public DesignTimeProvider(ServiceProvider provider, DbContext dbContext)
        {
            Provider = provider;
            DbContext = dbContext;
        }

        public ServiceProvider Provider { get; }

        public DbContext DbContext { get; }

        public void Dispose()
        {
            Provider.Dispose();
            DbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public object? GetService(Type serviceType)
        {
            return Provider.GetService(serviceType);
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

        public DesignTimeProvider GetDesignTimeProvider(IModel? model = null, Action<IServiceCollection>? config = null)
        {
            var dbc = GetDbContext(model);
            var services = new ServiceCollection();
            services.AddEntityFrameworkDesignTimeServices();
            services.AddDbContextDesignTimeServices(dbc);
            //ScaffoldHelper.ConfigServices(services);
            config?.Invoke(services);
            var provider = services.BuildServiceProvider();
            return new DesignTimeProvider(provider, dbc);
        }
        public DbContext GetDbContext(IModel? model = null)
        {
            var opt = new DbContextOptionsBuilder();
            BuilderConfig(opt);
            if (model != null)
            {
                opt.UseModel(model);
            }
            var opts = opt.Options;
            opts.GetExtension<CoreOptionsExtension>().WithServiceProviderCachingEnabled(false);
            return new DbContext(opts);
        }
        public bool EnsureDatabaseCreated()
        {
            if (dbContext == null)
            {
                dbContext = GetDbContext();
            }
            return dbContext!.Database.EnsureCreated();
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
}
