using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.Sqlite
{
    [SuppressMessage("", "EF1001")]
    public class SqliteStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "sqlite.OptionAction";

        public static readonly SqliteStockIntangible Default = new SqliteStockIntangible(ConnectStringStockIntangible.Instance);

        public SqliteStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            : base(connectStringStockIntangible)
        {
        }

        public override string EngineCode => KnowsDbCode.Sqlite;

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var optionAction = context.GetOrDefault<Action<SqliteDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseSqlite(box.ConnectionString, optionAction);
        }

        protected override void ConfigDesignTimeServices(ConnectionStringBox box1, DesignTimeServiceBox box2, IIntangibleContext? context)
        {
            new SqliteDesignTimeServices().ConfigureDesignTimeServices(box2.Services);
        }

        protected override void ConfigMigrationRunnerBuilder(ConnectionStringBox box, IMigrationRunnerBuilder builder, IIntangibleContext? context)
        {
            builder.AddSQLite().WithGlobalConnectionString(box.ConnectionString);
        }

        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new SqliteConnection(box.ConnectionString);
        }

        protected override SQLArchitectureQuerying CreateSQLArchitectureQuerying(ConnectionStringBox box, IIntangibleContext? context)
        {
            return DelegateSQLArchitectureQuerying.Sqlite(CreateDbConnection(box, context));
        }
    }
}
