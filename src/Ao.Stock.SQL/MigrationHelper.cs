using Ao.Stock.Comparering;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Ao.Stock.SQL
{
    public class MigrationHelper
    {
        public MigrationHelper(IReadOnlyList<IStockComparisonAction> actions)
        {
            Actions = actions;
        }

        public IReadOnlyList<IStockComparisonAction> Actions { get; }

        protected virtual void ConfigMigrationRunnerBuilder(IMigrationRunnerBuilder builder)
        {

        }
        protected virtual void ConfigServices(IServiceCollection services)
        {
            services.AddLogging(lb => lb.AddFluentMigratorConsole());
        }
        public void Migrate()
        {
            using (var provider=CreateProvider())
            {
                Migrate(provider);
            }
        }
        public void Migrate(IServiceProvider provider)
        {
            var runner = provider.GetRequiredService<IMigrationRunner>();
            runner.Up(new DynamicMigration(Actions));
        }
        public ServiceProvider CreateProvider()
        {
            var services= new ServiceCollection()
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
