using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.PostgreSql
{
    [SuppressMessage("", "EF1001")]
    public class PostgreSqlStockIntangible : SQLStockIntangible
    {
        public const string OptionActionKey = "postgresql.OptionAction";

        public static readonly PostgreSqlStockIntangible Default = new PostgreSqlStockIntangible(ConnectStringStockIntangible.Instance);

        public PostgreSqlStockIntangible(ConnectStringStockIntangible connectStringStockIntangible)
            : base(connectStringStockIntangible)
        {
        }

        public override string EngineCode => KnowsDbCode.PostgrSql;

        protected override void ConfigDbOptionBuilder(ConnectionStringBox box, DbContextOptionsBuilder builder, IIntangibleContext? context)
        {
            var optionAction = context.GetOrDefault<Action<NpgsqlDbContextOptionsBuilder>>(OptionActionKey);
            builder.UseNpgsql(box.ConnectionString, optionAction);
        }
        protected override void ConfigDesignTimeServices(ConnectionStringBox box1, DesignTimeServiceBox box2, IIntangibleContext? context)
        {
            new NpgsqlDesignTimeServices().ConfigureDesignTimeServices(box2.Services);
        }

        protected override void ConfigMigrationRunnerBuilder(ConnectionStringBox box, IMigrationRunnerBuilder builder, IIntangibleContext? context)
        {
            builder.AddPostgres().WithGlobalConnectionString(box.ConnectionString);
        }

        protected override DbConnection CreateDbConnection(ConnectionStringBox box, IIntangibleContext? context)
        {
            return new NpgsqlConnection(box.ConnectionString);
        }
        protected override SQLArchitectureQuerying CreateSQLArchitectureQuerying(ConnectionStringBox box, IIntangibleContext? context)
        {
            return DelegateSQLArchitectureQuerying.PostgreSql(CreateDbConnection(box, context));
        }
    }
}
