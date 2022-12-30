using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Ao.Stock.SQL
{
    public abstract class SQLStockIntangible : IStockIntangible
    {
        public const string ConnectionStringKey = "ConnectionString";

        public SQLStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
        {
            ConnectStringStockIntangible = connectStringStockIntangible ?? throw new ArgumentNullException(nameof(connectStringStockIntangible));
        }

        public ConnectStringStockIntangible ConnectStringStockIntangible { get; }

        public ConnectionStringBox? GetConnectionStringBox(IIntangibleContext? context)
        {
            if (context.TryGetValue(ConnectionStringKey, out var strObj) && strObj is string str)
            {
                return new ConnectionStringBox(str, null);
            }
            return ConnectStringStockIntangible.Get<ConnectionStringBox>(context);
        }

        public virtual void Config<T>(ref T input, IIntangibleContext? context)
        {
            if (typeof(DbContextOptionsBuilder).IsAssignableFrom(typeof(T)))
            {
                var builder = (DbContextOptionsBuilder)(object)input!;
                var connBox = GetConnectionStringBox(context);
                ConfigDbOptionBuilder(connBox, builder, context);
            }
            else if (typeof(T) == typeof(IMigrationRunnerBuilder))
            {
                var builder = (IMigrationRunnerBuilder)(object)input!;
                var connBox = GetConnectionStringBox(context);
                ConfigMigrationRunnerBuilder(connBox, builder, context);
            }
            else if (typeof(T) == typeof(DesignTimeServiceBox))
            {
                var builder = (DesignTimeServiceBox)(object)input!;
                var connBox = GetConnectionStringBox(context);
                ConfigDesignTimeServices(connBox, builder, context);
            }
        }
        protected abstract void ConfigDesignTimeServices(ConnectionStringBox box1, DesignTimeServiceBox box2, IIntangibleContext? context);

        protected abstract void ConfigMigrationRunnerBuilder(ConnectionStringBox box, IMigrationRunnerBuilder builder, IIntangibleContext? context);

        protected abstract void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context);

        protected abstract DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context);
         
        public virtual T Get<T>(IIntangibleContext? context)
        {
            if (typeof(T) == typeof(DbContext))
            {
                var builder = new DbContextOptionsBuilder();
                Config(ref builder, context);
                return (T)(object)new DbContext(builder.Options);
            }
            if (typeof(T) == typeof(IDbConnection)|| typeof(T) == typeof(DbConnection))
            {
                var connBox = GetConnectionStringBox(context);
                return (T)(object)CreateDbConnection(connBox, context);
            }
            if (typeof(T) == typeof(AutoMigrationHelper))
            {
                return (T)(object)new AutoMigrationHelperBuilder()
                    .WithBuilderConfig(x => Config(ref x, context))
                    .WithMigration(x => Config(ref x, context))
                    .WithScaffold(Get<DbConnection>(context), x =>
                    {
                        var designServiceBox = new DesignTimeServiceBox(x);
                        Config(ref designServiceBox, context);
                    })
                    .Build();
            }
            return default;
        }
    }
}
