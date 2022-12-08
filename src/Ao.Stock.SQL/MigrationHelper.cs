using Ao.Stock.Comparering;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Ao.Stock.SQL
{
    public class DefaultMigrationHelper : MigrationHelper
    {
        public DefaultMigrationHelper(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string ConnectionString { get; }

        protected override void ConfigMigrationRunnerBuilder(IMigrationRunnerBuilder builder)
        {
            builder.AddFirebird()
                .AddSqlServer()
                .AddMySql5()
                .AddSQLite()
                .AddSqlAnywhere()
                .AddPostgres()
                .AddOracle()
                .AddDb2();
        }
    }
    public class MigrationHelper
    {
        protected virtual void ConfigMigrationRunnerBuilder(IMigrationRunnerBuilder builder)
        {

        }
        protected virtual void ConfigServices(IServiceCollection services)
        {
            services.AddLogging(lb => lb.AddFluentMigratorConsole());
        }
        public void Migrate(IReadOnlyList<IStockComparisonAction> actions)
        {
            using (var provider = CreateProvider())
            {
                Migrate(actions,provider);
            }
        }
        public void Migrate(IReadOnlyList<IStockComparisonAction> actions,IServiceProvider provider)
        {
            var runner = provider.GetRequiredService<IMigrationRunner>();
            runner.Up(new DynamicMigration(actions));
        }
        public ServiceProvider CreateProvider()
        {
            var services = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    ConfigMigrationRunnerBuilder(rb);
                });
            ConfigServices(services);
            return services.BuildServiceProvider(false);
        }
    }
}
