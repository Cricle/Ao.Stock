using FluentMigrator.Runner;

namespace Ao.Stock.SQL
{
    public class DefaultMigrationHelper : MigrationHelper
    {
        public DefaultMigrationHelper(Action<IMigrationRunnerBuilder> runnerBuilderAction)
        {
            RunnerBuilderAction = runnerBuilderAction ?? throw new ArgumentNullException(nameof(runnerBuilderAction));
        }

        public Action<IMigrationRunnerBuilder> RunnerBuilderAction { get; }

        protected override void ConfigMigrationRunnerBuilder(IMigrationRunnerBuilder builder)
        {
            RunnerBuilderAction(builder);
        }
    }
}
