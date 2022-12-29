using Ao.Stock.Comparering;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Ao.Stock.SQL.PostgreSql
{
    [SuppressMessage("", "EF1001")]
    public class PostgreSqlAutoMigrateRunner : IAutoMigrateRunner
    {
        public PostgreSqlAutoMigrateRunner(string connectionString, IStockType newStockType, string tableName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            NewStockType = newStockType;
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string ConnectionString { get; }

        public IStockType NewStockType { get; }

        public string TableName { get; }

        public Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? Project { get; set; }

        public void Migrate()
        {
            using (var auto = new AutoMigrationHelperBuilder()
                .WithBuilderConfig(x => x.UseNpgsql(ConnectionString))
                .WithMigration(x => x.AddPostgres().WithGlobalConnectionString(ConnectionString))
                .WithScaffold(new NpgsqlConnection(ConnectionString), x => new NpgsqlDesignTimeServices().ConfigureDesignTimeServices(x))
                .Build())
            {
                auto.EnsureDatabaseCreated();
                auto.Begin(NewStockType)
                    .ScaffoldCompareAndMigrate(TableName, Project);
            }
        }

    }
}
