using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
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
}
