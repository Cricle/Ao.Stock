namespace Ao.Stock.SQL
{
    public struct AutoMigrateOptions
    {
        public AutoMigrateOptions(bool onlyDataBaseCreated)
        {
            OnlyDataBaseCreated = onlyDataBaseCreated;
        }

        public bool OnlyDataBaseCreated { get; set; }
    }
    public interface IAutoMigrateRunner
    {
        void Migrate(AutoMigrateOptions options=default);
    }
}
